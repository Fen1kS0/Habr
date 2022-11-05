namespace Habr.Common.DTOs.V1.Auth;

public class RefreshRequest
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}