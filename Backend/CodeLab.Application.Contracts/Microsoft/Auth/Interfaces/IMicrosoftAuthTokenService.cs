using CodeLab.Application.Contracts.Microsoft.Auth.DTOs;

namespace CodeLab.Application.Contracts.Microsoft.Auth.Interfaces;

public interface IMicrosoftAuthTokenService
{
    Task<MicrosoftToken> GetAccessToken(string authCode);
}