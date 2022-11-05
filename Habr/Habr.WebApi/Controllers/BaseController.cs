using System.Security.Claims;
using Habr.WebApi.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Habr.WebApi.Controllers;

[ApiController]
[Authorize(Policy = PolicyNames.RequireUserRole)]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected int UserId => !User.Identity.IsAuthenticated
        ? default
        : int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
}