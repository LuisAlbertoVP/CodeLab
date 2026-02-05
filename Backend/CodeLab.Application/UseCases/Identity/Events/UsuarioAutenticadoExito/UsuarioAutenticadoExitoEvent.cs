using CodeLab.Application.Shared.Common;

namespace CodeLab.Application.UseCases.Identity.Events.UsuarioAutenticadoExito;

public record UsuarioAutenticadoExitoEvent(long IdUsuario) : INotification;