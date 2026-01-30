namespace CodeLab.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; set; }
    public int IdUsuario { get; set; }
    public string Token { get; set; }
    public DateTime FechaExpiracion { get; set; }
    public DateTime? FechaRevocacion { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
    public string? IpCreacion { get; set; }
    public string? Dispositivo { get; set; }
    public Usuarios? Usuario { get; set; }
}