namespace CodeLab.Infrastructure.Jwt.Contracts.Settings;

public class JwtSettings
{
    public string Secret { get; set; }

    public string Issuer { get; set; }

    public string Audience { get; set; }

    public int ExpiryMinutes { get; set; }
}