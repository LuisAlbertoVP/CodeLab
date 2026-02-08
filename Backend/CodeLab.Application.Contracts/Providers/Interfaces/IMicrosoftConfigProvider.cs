namespace CodeLab.Application.Contracts.Providers.Interfaces;

public interface IMicrosoftConfigProvider
{
    string TenantId { get; }
    string ClientId { get; }
    string ClientSecret { get; }
    string Scopes { get; }
    string RedirectUrl { get; }
    string UrlAccessToken { get; }
}