using Microsoft.Kiota.Abstractions.Authentication;

namespace CodeLab.Infrastructure.Microsoft.Auth.Providers;

public class TokenProvider(string token) : IAccessTokenProvider
{
    public AllowedHostsValidator AllowedHostsValidator => throw new NotImplementedException();

    public Task<string> GetAuthorizationTokenAsync(Uri uri, Dictionary<string, object> additionalAuthenticationContext = default, CancellationToken ct = default)
    {
        return Task.FromResult(token);
    }
}