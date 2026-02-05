using System.Diagnostics;
using CodeLab.Application.Interfaces.Logging;
using CodeLab.Application.Shared.Common;

namespace CodeLab.Application.Shared.Behavior;

public class LoggingBehavior<TInput, TOutput>(ICodeLabLogger logger) : IPipelineBehavior<TInput, TOutput>
{
    private readonly Stopwatch timer = new();

    public async Task<TOutput> Handle(TInput input, Func<Task<TOutput>> next, CancellationToken ct = default)
    {
        timer.Start();
        logger.LogInformation($"Manejando request: {typeof(TInput).Name}");
        
        var response = await next();

        timer.Stop();
        logger.LogInformation($"Request {typeof(TInput).Name} procesada en {timer.ElapsedMilliseconds}ms");

        return response;
    }
}