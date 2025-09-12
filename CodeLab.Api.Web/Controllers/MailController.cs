using CodeLab.Application.Identity.Commands.EnviarMailBienvenida;
using CodeLab.Application.Shared.Common;
using CodeLab.Application.Shared.Results;
using Microsoft.AspNetCore.Mvc;

namespace CodeLab.Api.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MailController(IMediator mediator) : ControllerBase
{
    [HttpGet("EnviarMailBienvenida")]
    public async Task<IActionResult> EnviarMailBienvenida([FromQuery] EnviarMailBienvenidaCommand command)
    {
        var resultado = await mediator.Send<EnviarMailBienvenidaCommand, CodeLabResultado<string>>(command);
        if (!resultado.EsExito)
        {
            return StatusCode(500, resultado.MensajeError);
        }

        return Ok(new { respuesta = resultado.Valor });
    }
}