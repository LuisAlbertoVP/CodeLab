using CodeLab.Application.Contracts.Microsoft.Auth.DTOs;

namespace CodeLab.Application.Contracts.Microsoft.Auth.Interfaces;

public interface IMicrosoftAuthService
{
    Task<MicrosoftUser> GetMicrosoftGraphUser(string authCode);
}