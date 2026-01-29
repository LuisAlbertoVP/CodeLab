namespace CodeLab.Application.Shared.Common;

public interface ISender
{
    Task<TResult> Send<TRequest, TResult>(TRequest request, CancellationToken cancellationToken = default) 
        where TRequest : IRequest<TResult>;
}