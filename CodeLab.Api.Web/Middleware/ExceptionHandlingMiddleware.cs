using System.Net;
using CodeLab.Application.Shared.Exceptions;
using CodeLab.Infrastructure.Logging.Contracts.Interfaces;

namespace CodeLab.Api.Web.Middleware;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ICodeLabLogger logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (CodeLabException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)ex.HttpStatusCode;

            var errorResponse = new
            {
                context.Response.StatusCode,
                Mensaje = ex.Message
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
        catch (Exception ex)
        {
            logger.LogError("Se produjo un error no controlado", ex);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorResponse = new
            {
                context.Response.StatusCode,
                Mensaje = "Ocurrió un problema al procesar la solicitud. Intente nuevamente más tarde."
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}