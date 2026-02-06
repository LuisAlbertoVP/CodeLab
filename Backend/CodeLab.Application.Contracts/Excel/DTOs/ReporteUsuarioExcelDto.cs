namespace CodeLab.Application.Contracts.Excel.DTOs;

public class ReporteUsuarioExcelDto
{
    public int Id { get; set; }

    public string Nombre { get; set; }

    public string Email { get; set; }

    public DateTime FechaRegistro { get; set; }
}