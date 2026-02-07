using CodeLab.Domain.Enums;

namespace CodeLab.Domain.Entities;

public class UsuarioCanal
{
    public long Id { get; set; }
    public int IdUsuario { get; set; }
    public OutboundChannel Canal { get; set; }
    public string Destino { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaModificacion { get; set; }
}