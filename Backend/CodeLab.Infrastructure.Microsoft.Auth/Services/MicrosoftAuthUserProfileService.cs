using CodeLab.Application.Contracts.Microsoft.Auth.DTOs;
using CodeLab.Application.Contracts.Microsoft.Auth.Interfaces;
using CodeLab.Infrastructure.Microsoft.Auth.Providers;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Kiota.Abstractions.Authentication;

namespace CodeLab.Infrastructure.Microsoft.Auth.Services;

public class MicrosoftAuthUserProfileService : IMicrosoftAuthUserProfileService
{
    private GraphServiceClient? graphClient;

    public void CreateClient(string accessToken)
    {
        var provider = new TokenProvider(accessToken);
        var authProvider = new BaseBearerTokenAuthenticationProvider(provider);
        graphClient = new GraphServiceClient(authProvider);
    }

    public async Task<MicrosoftUser> GetUser()
    {
        if (graphClient == null)
            throw new InvalidOperationException("Graph client no ha sido inicializado. Llama a CreateClient primero.");

        var user = await graphClient.Me.GetAsync();
        return new MicrosoftUser
        {
            Nombre = user.GivenName,
            Apellido = user.Surname,
            Correo = user.Mail
        };
    }

    public async Task<string?> GetUserPhotoBase64()
    {
        if (graphClient == null)
            throw new InvalidOperationException("Graph client no ha sido inicializado. Llama a CreateClient primero.");

        try
        {
            var photoStream = await graphClient.Me.Photo.Content.GetAsync();
            using var ms = new MemoryStream();
            await photoStream.CopyToAsync(ms);
            var photoBytes = ms.ToArray();
            return Convert.ToBase64String(photoBytes);
        }
        catch (Exception)
        {
            // Ignorar errores y devolver null o string vacío si no hay foto
            return null;
        }
    }

    public async Task<List<string?>> GetUserRoles()
    {
        if (graphClient == null)
            throw new InvalidOperationException("Graph client no ha sido inicializado. Llama a CreateClient primero.");

        var directoryObjectsResponse = await graphClient.Me.MemberOf.GetAsync(requestConfiguration =>
        {
            requestConfiguration.QueryParameters.Top = 200;
        });

        var directoryObjects = directoryObjectsResponse?.Value?.OfType<Group>().Select(g => g.DisplayName);
        return directoryObjects?.ToList() ?? [];
    }
}