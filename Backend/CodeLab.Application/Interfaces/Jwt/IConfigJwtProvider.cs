namespace CodeLab.Application.Interfaces.Jwt;

public interface IConfigJwtProvider
{
    string Secret { get; }

    string Issuer { get; }

    string Audience { get; }

    int ExpiryMinutes { get; }
}