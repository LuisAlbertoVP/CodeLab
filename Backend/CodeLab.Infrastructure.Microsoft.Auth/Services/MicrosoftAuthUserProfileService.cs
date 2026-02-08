using CodeLab.Application.Contracts.Microsoft.Auth.DTOs;
using CodeLab.Application.Contracts.Microsoft.Auth.Interfaces;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace CodeLab.Infrastructure.Microsoft.Auth.Services;

public class MicrosoftAuthUserProfileService(GraphServiceClient client) : IMicrosoftAuthUserProfileService
{
    public async Task<MicrosoftUser> GetUser()
    {
        var user = await client.Me.GetAsync();
        return new MicrosoftUser
        {
            Nombre = user.GivenName,
            Apellido = user.Surname,
            Correo = user.Mail
        };
    }

    public async Task<string> GetUserPhotoBase64()
    {
        try
        {
            var photoStream = await client.Me.Photo.Content.GetAsync();
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

    public async Task<List<string>> GetUserRoles()
    {
        var directoryObjectsResponse = await client.Me.MemberOf.GetAsync(requestConfiguration =>
        {
            requestConfiguration.QueryParameters.Top = 200;
        });

        var directoryObjects = directoryObjectsResponse.Value.OfType<Group>().Select(g => g.DisplayName);
        return directoryObjects.ToList();
    }
}