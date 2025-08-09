using System.Text;
using CodeLab.Application.Identity.Commands.Autenticar;
using CodeLab.Application.Identity.Commands.RefrescarToken;
using CodeLab.Application.Shared.Common;
using CodeLab.Application.Shared.Results;
using Microsoft.AspNetCore.Mvc;

namespace CodeLab.Api.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IMediator mediator) : ControllerBase
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

        var comando = new AutenticarCommand(parts[0], parts[1]);
        var resultado = await mediator.Send<AutenticarCommand, CodeLabResultado<LoginResultDTO>>(comando);
        if (!resultado.EsExito)
        {
            return Unauthorized(resultado.MensajeError);
        }

        Response.Cookies.Append("RefreshToken", resultado.Valor.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = resultado.Valor.Expiration
        });

        return Ok(new { token = resultado.Valor.Token });
    }

    [HttpPost("RefrescarToken")]
    public async Task<IActionResult> RefrescarToken()
    {
        var refreshToken = Request.Cookies["RefreshToken"];
        if (string.IsNullOrWhiteSpace(refreshToken))
            return Unauthorized("No se encontró el Refresh Token");

        var comando = new RefrescarTokenCommand(refreshToken);
        var resultado = await mediator.Send<RefrescarTokenCommand, CodeLabResultado<LoginResultDTO>>(comando);
        if (!resultado.EsExito)
        {
            return Unauthorized(resultado.MensajeError);
        }

        Response.Cookies.Append("RefreshToken", resultado.Valor.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = resultado.Valor.Expiration
        });

        return Ok(new { token = resultado.Valor.Token });
    }
}