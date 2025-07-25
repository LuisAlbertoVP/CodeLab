using System.Text;
using CodeLab.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CodeLab.Api.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService service) : ControllerBase
{
    [HttpGet("IniciarSesion")]
    public async Task<IActionResult> IniciarSesion()
    {
        var authHeader = Request.Headers["Authorization"].ToString();

        if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Basic "))
            return Unauthorized("No se encontró la cabecera de autenticación");

        var encodedCredentials = authHeader["Basic ".Length..].Trim();

        var decodedBytes = Convert.FromBase64String(encodedCredentials);
        var decodedString = Encoding.UTF8.GetString(decodedBytes);

        var parts = decodedString.Split(':', 2);
        if (parts.Length != 2)
            return Unauthorized("Credenciales mal formateadas");

        var email = parts[0];
        var clave = parts[1];

        var token = await service.IniciarSesion(email, clave);
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized("Credenciales inválidas");
        }
        return Ok(token);
    }
}