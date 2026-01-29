using CodeLab.Application.Shared.Common;

namespace CodeLab.Application.Identity.Events.UsuarioAutenticadoExito;

public record UsuarioAutenticadoExitoEvent(long IdUsuario) : INotification;