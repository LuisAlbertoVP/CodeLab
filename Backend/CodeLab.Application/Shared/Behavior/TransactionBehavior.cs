using CodeLab.Application.Shared.Common;
using CodeLab.Domain.Interfaces;

namespace CodeLab.Application.Shared.Behavior;

public class TransactionBehavior<TInput, TOutput>(IUnitOfWork unitOfWork) : IPipelineBehavior<TInput, TOutput>
    where TInput : ICommand<TOutput>
{
    public async Task<TOutput> Handle(TInput input, Func<Task<TOutput>> next, CancellationToken ct = default)
    {
        var response = await next();

        await unitOfWork.SaveChangesAsync(ct);

        return response;
    }
}