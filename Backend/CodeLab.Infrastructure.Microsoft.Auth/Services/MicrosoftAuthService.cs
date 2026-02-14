using CodeLab.Application.Contracts.Microsoft.Auth.DTOs;
using CodeLab.Application.Contracts.Microsoft.Auth.Interfaces;

namespace CodeLab.Infrastructure.Microsoft.Auth.Services;

public class MicrosoftAuthService(
    IMicrosoftAuthTokenService microsoftAuthTokenService,
    IMicrosoftAuthUserProfileService microsoftAuthUserProfileService
) : IMicrosoftAuthService
{
    public async Task<MicrosoftUser> GetMicrosoftGraphUser(string authCode)
    {
        var microsoftToken = await microsoftAuthTokenService.GetAccessToken(authCode);
        microsoftAuthUserProfileService.CreateClient(microsoftToken.AccessToken);

        var user = await microsoftAuthUserProfileService.GetUser();
        user.FotoBase64 = await microsoftAuthUserProfileService.GetUserPhotoBase64();
        var roles = await microsoftAuthUserProfileService.GetUserRoles();
        user.RolesActiveDirectory = roles;

        return user;
    }
}