using Habr.Common.DTOs.V1.Auth;
using Habr.Common.DTOs.V1.Users;

namespace Habr.BL.Interfaces.Services.V1;

public interface IUserService
{
    Task<UserResponse> GetUserByIdAsync(int id);
    Task<AuthResponse> LoginUserAsync(UserLoginRequest userLoginRequest);
    Task<AuthResponse> RegisterUserAsync(UserRegisterRequest userRegisterRequest);
    Task<AuthResponse> RefreshToken(RefreshRequest refreshRequest);
    Task RevokeToken(int userId);
}