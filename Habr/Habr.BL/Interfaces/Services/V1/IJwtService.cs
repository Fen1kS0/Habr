using System.Security.Claims;

namespace Habr.BL.Interfaces.Services.V1;

public interface IJwtService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string? GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string? token);
}