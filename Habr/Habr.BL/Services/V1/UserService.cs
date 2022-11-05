using System.Security.Claims;
using AutoMapper;
using Habr.BL.Interfaces.Services.V1;
using Habr.Common;
using Habr.Common.Configuration;
using Habr.Common.DTOs.V1.Auth;
using Habr.Common.DTOs.V1.Users;
using Habr.Common.Exceptions;
using Habr.Common.Resources;
using Habr.DataAccess.Entities;
using Habr.DataAccess.Interfaces.Repositories;
using Habr.DataAccess.UoW;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Habr.BL.Services.V1;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;
    private readonly IJwtService _jwtService;
    private readonly JwtConfig _jwtConfig;

    public UserService(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        ILogger<UserService> logger, 
        IJwtService jwtService, 
        JwtConfig jwtConfig
        )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _jwtService = jwtService;
        _jwtConfig = jwtConfig;
    }

    public async Task<UserResponse> GetUserByIdAsync(int id)
    {
        var user = await _unitOfWork.GetRepository<IUserRepository>().GetEntityByIdAsync(id);
        if (user is null)
        {
            throw new NotFoundException(ExceptionMessageResource.UserNotFound);
        }

        return _mapper.Map<UserResponse>(user);
    }

    public async Task<AuthResponse> LoginUserAsync(UserLoginRequest userLoginRequest)
    {
        var user = await _unitOfWork.GetRepository<IUserRepository>().GetUserByEmail(userLoginRequest.Email);
        if (user is null)
        {
            throw new AuthenticationException(ExceptionMessageResource.InvalidEmailOrPassword);
        }

        if (!Hash.VerifyHashedPassword(user.Password, userLoginRequest.Password))
        {
            throw new AuthenticationException(ExceptionMessageResource.InvalidEmailOrPassword);
        }

        _logger.LogInformation(InformationMessageResource.UserLogin);
        
        var claims = GenerateUserClaims(user);

        string accessToken = _jwtService.GenerateAccessToken(claims);
        string? refreshToken = _jwtService.GenerateRefreshToken();
        
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtConfig.RefreshTokenExpiryInDays);
        await _unitOfWork.GetRepository<IUserRepository>().UpdateEntityAsync(user);

        return new AuthResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<AuthResponse> RegisterUserAsync(UserRegisterRequest userRegisterRequest)
    {
        var userByEmail = await _unitOfWork.GetRepository<IUserRepository>().GetUserByEmail(userRegisterRequest.Email);
        if (userByEmail is not null)
        {
            throw new BusinessException(ExceptionMessageResource.EmailBusy);
        }

        var user = _mapper.Map<User>(userRegisterRequest);
        string? refreshToken = _jwtService.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtConfig.RefreshTokenExpiryInDays);
        
        await _unitOfWork.GetRepository<IUserRepository>().AddEntityAsync(user);
        _logger.LogInformation(InformationMessageResource.UserRegister);
        
        var claims = GenerateUserClaims(user);
        string accessToken = _jwtService.GenerateAccessToken(claims);
        return new AuthResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<AuthResponse> RefreshToken(RefreshRequest refreshRequest)
    {
        string? accessToken = refreshRequest.AccessToken;
        string? refreshToken = refreshRequest.RefreshToken;

        var principal = _jwtService.GetPrincipalFromExpiredToken(accessToken);
        var userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var user = await _unitOfWork.GetRepository<IUserRepository>().GetEntityByIdAsync(
            userId,
            disableTracking: false
        );

        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            throw new BusinessException("Invalid refresh token request");
        }

        var newAccessToken = _jwtService.GenerateAccessToken(principal.Claims);
        var newRefreshToken = _jwtService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtConfig.RefreshTokenExpiryInDays);
        await _unitOfWork.GetRepository<IUserRepository>().UpdateEntityAsync(user);

        return new AuthResponse()
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

    public async Task RevokeToken(int userId)
    {
        var user = await _unitOfWork.GetRepository<IUserRepository>().GetEntityByIdAsync(userId, disableTracking: false);
        if (user is null)
        {
            throw new AuthenticationException(ExceptionMessageResource.UserNotFound);
        }

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;

        await _unitOfWork.GetRepository<IUserRepository>().UpdateEntityAsync(user);
    }

    private IEnumerable<Claim> GenerateUserClaims(User user)
    {
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role)
        };

        return claims;
    }
}