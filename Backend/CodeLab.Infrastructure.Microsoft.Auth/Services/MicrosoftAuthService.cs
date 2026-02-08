using CodeLab.Application.Contracts.Microsoft.Auth.DTOs;
using CodeLab.Application.Contracts.Microsoft.Auth.Interfaces;
using System.Text.Json;

namespace CodeLab.Infrastructure.Microsoft.Auth.Services;

public class MicrosoftAuthService(
    IMicrosoftAuthTokenService microsoftAuthTokenService,
    IMicrosoftGraphClientFactory<IMicrosoftAuthUserProfileService> graphClientFactory
) : IMicrosoftAuthService
{
    public async Task<MicrosoftUser> GetMicrosoftGraphUser(string authCode)
    {
        var microsoftToken = await microsoftAuthTokenService.GetAccessToken(authCode);
        var graphClient = graphClientFactory.CreateClient(microsoftToken.AccessToken);

        var user = await graphClient.GetUser();
        user.FotoBase64 = await graphClient.GetUserPhotoBase64();
        var roles = await graphClient.GetUserRoles();
        user.RolesActiveDirectory = JsonSerializer.Serialize(roles);

        return user;
    }
}