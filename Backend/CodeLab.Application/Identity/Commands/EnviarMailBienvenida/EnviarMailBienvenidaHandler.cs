using CodeLab.Application.Shared.Common;
using CodeLab.Application.Shared.Results;
using CodeLab.Infrastructure.RabbitMq.Contracts.Interfaces;

namespace CodeLab.Application.Identity.Commands.EnviarMailBienvenida;

public class EnviarMailBienvenidaHandler(IMailPublisherService mailPublisherService) : IRequestHandler<EnviarMailBienvenidaCommand, CodeLabResultado<string>>
{
    public async Task<CodeLabResultado<string>> Handle(EnviarMailBienvenidaCommand request, CancellationToken ct)
    {
        await mailPublisherService.PublishAsync(request.Mensaje);
        return CodeLabResultado<string>.Exito("Mail de bienvenida enviado");
    }
}