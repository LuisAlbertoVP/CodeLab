using CodeLab.Application.Interfaces.Jwt;
using Microsoft.Extensions.Configuration;

namespace CodeLab.Infrastructure.Config;

public class ConfigJwtProvider(IConfiguration configuration) : IConfigJwtProvider
{
    public string Secret => configuration["JwtSettings:Secret"];

    public string Issuer => configuration["JwtSettings:Issuer"];

    public string Audience => configuration["JwtSettings:Audience"];

    public int ExpiryMinutes => int.Parse(configuration["JwtSettings:ExpiryMinutes"]);
}