namespace CodeLab.Application.Shared.Common;

public interface IPipelineBehavior<TInput, TOutput>
{
    Task<TOutput> Handle(TInput input, Func<Task<TOutput>> next, CancellationToken ct = default);
}