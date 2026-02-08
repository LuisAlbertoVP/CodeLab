using CodeLab.Application.Contracts.Microsoft.Auth.Interfaces;
using CodeLab.Infrastructure.Microsoft.Auth.Providers;
using Microsoft.Graph;
using Microsoft.Kiota.Abstractions.Authentication;

namespace CodeLab.Infrastructure.Microsoft.Auth.Services;

public class MicrosoftAuthUserProfileServiceFactory : IMicrosoftGraphClientFactory<IMicrosoftAuthUserProfileService>
{
    public IMicrosoftAuthUserProfileService CreateClient(string accessToken)
    {
        var provider = new TokenProvider(accessToken);
        var authProvider = new BaseBearerTokenAuthenticationProvider(provider);
        var graphClient = new GraphServiceClient(authProvider);
        return new MicrosoftAuthUserProfileService(graphClient);
    }
}