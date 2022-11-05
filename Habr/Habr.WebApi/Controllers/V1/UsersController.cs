using Habr.BL.Interfaces.Services.V1;
using Habr.Common.DTOs.V1.Auth;
using Habr.Common.DTOs.V1.Users;
using Habr.DataAccess.Entities;
using Habr.WebApi.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Habr.WebApi.Controllers.V1;

[ApiVersion("1.0")]
[ApiVersion("2.0")]
[ApiVersion("3.0")]
public class UsersController : BaseController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserById([FromRoute] int id)
    {
        return Ok(await _userService.GetUserByIdAsync(id));
    }
    
    [HttpPost("sign-in")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginUser([FromBody] UserLoginRequest userLoginRequest)
    {
        return Ok(await _userService.LoginUserAsync(userLoginRequest));
    }
    
    [HttpPost("sign-up")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegisterRequest userRegisterRequest)
    {
        return Ok(await _userService.RegisterUserAsync(userRegisterRequest));
    }
    
    [HttpPost("refreshToken")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshRequest refreshRequest)
    {
        return Ok(await _userService.RefreshToken(refreshRequest));
    }
    
    [HttpPost("revokeToken")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RevokeToken()
    {
        await _userService.RevokeToken(UserId);
        
        return Accepted();
    }
    
    [HttpGet("myName")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrentName()
    {
        var user = await _userService.GetUserByIdAsync(UserId);
        
        return Ok(user.Name);
    }
    
    [HttpGet("allExistRoles")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetAllRoles()
    {
        return Ok(RoleNames.AllRoles);
    }

    [HttpGet("claims")]
    [Authorize(Policy = PolicyNames.RequireAdministratorRole)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetClaims()
    {
        var claims = HttpContext.User.Claims;
        
        return Ok(claims.Select(c => new
        {
            Type = c.Type,
            Value = c.Value
        }));
    }
}