using CodeLab.Application.Contracts.Http.Interfaces;
using CodeLab.Application.Contracts.Microsoft.Auth.DTOs;
using CodeLab.Application.Contracts.Microsoft.Auth.Exceptions;
using CodeLab.Application.Contracts.Microsoft.Auth.Interfaces;
using CodeLab.Application.Contracts.Providers.Interfaces;
using System.Text.Json;

namespace CodeLab.Infrastructure.Microsoft.Auth.Services;

public class MicrosoftAuthTokenService(
    IMicrosoftConfigProvider microsoftConfigProvider,
    IHttpClientService httpClientService
) : IMicrosoftAuthTokenService
{
    public async Task<MicrosoftToken> GetAccessToken(string authCode)
    {
        var url = microsoftConfigProvider.UrlAccessToken;
        url = url.Replace("{TENANT_ID}", microsoftConfigProvider.TenantId);
        var collection = new List<KeyValuePair<string, string>>
        {
            new("client_id", microsoftConfigProvider.ClientId),
            new("scope", microsoftConfigProvider.Scopes),
            new("code", authCode),
            new("redirect_uri", microsoftConfigProvider.RedirectUrl),
            new("grant_type", "authorization_code"),
            new("client_secret", microsoftConfigProvider.ClientSecret)
        };
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = new FormUrlEncodedContent(collection);
        var httpClient = httpClientService.GetHttpClient();
        var response = await httpClient.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new MicrosoftNoTokenException(json);
        }
        var token = JsonSerializer.Deserialize<MicrosoftToken>(json);
        return token;
    }
}