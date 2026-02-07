using CodeLab.Domain.Enums;

namespace CodeLab.Domain.Entities;

public class OutboundMessage
{
    public long Id { get; set; }
    public OutboundChannel Canal { get; set; }
    public string Destino { get; set; }
    public OutboundMessageType Tipo { get; set; }
    public string Mensaje { get; set; }
    public OutboundMessageStatus Estado { get; set; }
    public int CantidadReintentos { get; set; }
    public int MaxCantidadReintentos { get; set; }
    public DateTime? FechaSiguienteReintento { get; set; }
    public string? UltimoError { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaModificacion { get; set; }
}