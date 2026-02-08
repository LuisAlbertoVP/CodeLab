using System.Net.Http.Headers;
using System.Text;
using CodeLab.Application.Contracts.Http.Interfaces;

namespace CodeLab.Infrastructure.Http.Services;

public class HttpClientService(IHttpClientFactory clientFactory) : IHttpClientService
{
    public HttpClient GetHttpClient()
    {
        return clientFactory.CreateClient();
    }

    public Task<HttpResponseMessage> SendWithBasicAsync(string url, string username, string password)
    {
        var httpClient = clientFactory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic",
            Convert.ToBase64String(
                Encoding.ASCII.GetBytes($"{username}:{password}")
            )
        );
        return httpClient.SendAsync(request);
    }

    public Task<HttpResponseMessage> SendWithBearerAsync(HttpMethod method, string url, string token, HttpContent? content = null)
    {
        var httpClient = clientFactory.CreateClient();
        var request = new HttpRequestMessage(method, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        if (content != null)
            request.Content = content;
        return httpClient.SendAsync(request);
    }
}