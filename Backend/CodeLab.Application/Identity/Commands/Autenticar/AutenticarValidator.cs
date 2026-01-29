using FluentValidation;

namespace CodeLab.Application.Identity.Commands.Autenticar;

public class AutenticarValidator : AbstractValidator<AutenticarCommand>
{
    public AutenticarValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El campo {PropertyName} es obligatorio.")
            .EmailAddress().WithMessage("El campo {PropertyName} debe ser un email válido.");

        RuleFor(x => x.Clave)
            .NotEmpty().WithMessage("El campo {PropertyName} es obligatorio.");
    }
}