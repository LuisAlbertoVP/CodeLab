namespace CodeLab.Application.Contracts.Providers.Interfaces;

public interface IConfigJwtProvider
{
    string Secret { get; }

    string Issuer { get; }

    string Audience { get; }

    int ExpiryMinutes { get; }
}