namespace CodeLab.Domain.Entities;

public class UsuarioRol
{
    public int IdUsuario { get; set; }
    public int IdRol { get; set; }
    public DateTime FechaAsignacion { get; set; }
    public Usuarios? Usuario { get; set; }
    public Roles? Rol { get; set; }
}