using System;

namespace CodeLab.Infrastructure.Jwt.Contracts.DTOs;

public class JwtSettingsDto
{
    public string Secret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpiryMinutes { get; set; }
}