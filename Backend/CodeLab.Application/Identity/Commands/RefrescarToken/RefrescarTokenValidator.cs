using FluentValidation;

namespace CodeLab.Application.Identity.Commands.RefrescarToken;

public class RefrescarTokenValidator : AbstractValidator<RefrescarTokenCommand>
{
    public RefrescarTokenValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("El refresh token es obligatorio.");
    }
}