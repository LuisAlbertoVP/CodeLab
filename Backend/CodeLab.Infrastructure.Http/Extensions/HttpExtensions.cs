using Microsoft.Extensions.DependencyInjection;

namespace CodeLab.Infrastructure.Http.Extensions;

public static class HttpExtensions
{
    public static void AgregarHttpClientFactory(this IServiceCollection services, string nombre)
    {
        services
            .AddHttpClient(nombre)
            .SetHandlerLifetime(TimeSpan.FromMinutes(5))
            .ConfigurePrimaryHttpMessageHandler(_ =>
            {
                return new HttpClientHandler
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    // LÍNEA VULNERABLE (omitir validación de certificado del servidor)
                    // Esto aceptará cualquier certificado servidor aunque sea inválido, caducado,
                    // no coincida con el host o no esté firmado por una CA confiable.
                    ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true
                };
            });
    }
}