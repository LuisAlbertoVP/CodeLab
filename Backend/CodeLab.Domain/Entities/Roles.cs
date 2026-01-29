namespace CodeLab.Domain.Entities;

public class Roles
{
    public int Id { get; set; }
    public string? Codigo { get; set; }
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }
    public DateTime FechaCreacion { get; set; }
    public int UsuarioCreacion { get; set; }
}