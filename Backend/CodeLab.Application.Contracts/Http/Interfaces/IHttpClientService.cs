namespace CodeLab.Application.Contracts.Http.Interfaces;

public interface IHttpClientService
{
    HttpClient GetHttpClient();
    Task<HttpResponseMessage> SendWithBasicAsync(string url, string username, string password);
    Task<HttpResponseMessage> SendWithBearerAsync(HttpMethod method, string url, string token, HttpContent? content = null);
}