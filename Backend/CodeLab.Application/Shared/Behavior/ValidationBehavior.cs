using CodeLab.Application.Shared.Common;
using FluentValidation;

namespace CodeLab.Application.Shared.Behavior;

public class ValidationBehavior<TInput, TOutput>(IEnumerable<IValidator<TInput>> validators) : IPipelineBehavior<TInput, TOutput>
{
    public async Task<TOutput> Handle(TInput request, Func<Task<TOutput>> next, CancellationToken cancellationToken)
    {
        if (validators.Any())
        {
            var context = new ValidationContext<TInput>(request);
            var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
            if (failures.Any())
            {
                throw new ValidationException(failures);
            }
        }
        return await next();
    }
}