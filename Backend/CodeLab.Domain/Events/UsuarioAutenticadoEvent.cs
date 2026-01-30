using CodeLab.Domain.Entities;
using CodeLab.Domain.Interfaces;

namespace CodeLab.Domain.Events;

public class UsuarioAutenticadoEvent(Usuarios usuario) : IDomainEvent
{
    public Usuarios Usuario { get; } = usuario;
    public DateTime OcurredOn { get; } = DateTime.UtcNow;
}