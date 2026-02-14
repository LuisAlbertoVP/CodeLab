using CodeLab.Application.Contracts.Microsoft.Auth.DTOs;

namespace CodeLab.Application.Contracts.Microsoft.Auth.Interfaces;

public interface IMicrosoftAuthUserProfileService
{
    void CreateClient(string accessToken);
    Task<MicrosoftUser> GetUser();
    Task<string?> GetUserPhotoBase64();
    Task<List<string?>> GetUserRoles();
}