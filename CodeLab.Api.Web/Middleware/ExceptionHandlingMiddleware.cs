using System.Net;
using CodeLab.Infrastructure.Logging.Contracts.Interfaces;

namespace CodeLab.Api.Web.Middleware;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ICodeLabLogger logger,
    IHostEnvironment env)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError("Se produjo un error no controlado", ex);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorResponse = new
            {
                context.Response.StatusCode,
                Mensaje = env.IsDevelopment()
                    ? ex.Message
                    : "Ocurrió un problema al procesar la solicitud. Intente nuevamente más tarde."
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}