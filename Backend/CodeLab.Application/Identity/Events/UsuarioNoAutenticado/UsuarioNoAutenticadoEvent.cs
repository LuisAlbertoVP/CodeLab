using CodeLab.Application.Shared.Common;

namespace CodeLab.Application.Identity.Events.UsuarioNoAutenticado;

public record UsuarioNoAutenticadoEvent(string Usuario, Exception Exception) : INotification;