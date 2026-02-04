using System.Net;
using CodeLab.Application.Shared.Common;
using CodeLab.Application.Shared.Exceptions;
using CodeLab.Domain.Exceptions;
using CodeLab.Infrastructure.Logging.Contracts.Interfaces;
using FluentValidation;

namespace CodeLab.Application.Shared.Behavior;

public class ExceptionBehavior<TInput, TOutput>(ICodeLabLogger logger) : IPipelineBehavior<TInput, TOutput>
{
    public async Task<TOutput> Handle(TInput input, Func<Task<TOutput>> next, CancellationToken ct = default)
    {
        try
        {
            return await next();
        }
        catch (CodeLabException)
        {
            throw;
        }
        catch (ValidationException ex)
        {
            logger.LogError($"Request [{typeof(TInput).Name}]: {ex.Message}", ex);
            throw new CodeLabException(HttpStatusCode.BadRequest, ex.Message);
        }
        catch (DomainException ex)
        {
            throw new CodeLabException(HttpStatusCode.Conflict, ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError($"Request [{typeof(TInput).Name}]:", ex);
            throw new CodeLabException(HttpStatusCode.InternalServerError, "Ocurrió un problema al procesar la solicitud. Intente nuevamente más tarde.");
        }
    }
}