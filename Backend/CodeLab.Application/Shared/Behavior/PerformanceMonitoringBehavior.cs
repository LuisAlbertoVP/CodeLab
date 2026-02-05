using System.Diagnostics;
using CodeLab.Application.Interfaces.Logging;
using CodeLab.Application.Shared.Common;

namespace CodeLab.Application.Shared.Behavior;

public class PerformanceMonitoringBehavior<TInput, TOutput>(ICodeLabLogger logger) : IPipelineBehavior<TInput, TOutput>
{
    private readonly Stopwatch timer = new();

    public async Task<TOutput> Handle(TInput input, Func<Task<TOutput>> next, CancellationToken ct = default)
    {
        timer.Start();

        var response = await next();

        timer.Stop();

        var elapsedMilliseconds = timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
        {
            logger.LogWarning($"Lento: {typeof(TInput).Name} tomó {elapsedMilliseconds}ms.");
        }

        return response;
    }
}