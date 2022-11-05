namespace Habr.Common.Configuration;

public class JwtConfig
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SecretKey { get; set; }
    public int AccessTokenExpiryInMinutes { get; set; }
    public int RefreshTokenExpiryInDays { get; set; }
}