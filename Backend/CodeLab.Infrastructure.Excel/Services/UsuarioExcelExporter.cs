using ClosedXML.Excel;
using CodeLab.Infrastructure.Excel.Contracts.DTOs;
using CodeLab.Infrastructure.Excel.Contracts.Interfaces;

namespace CodeLab.Infrastructure.Excel.Services;

public class UsuarioExcelExporter : IExcelExporter<List<ReporteUsuarioExcelDto>>
{
    public ExcelFile ExportToExcel(List<ReporteUsuarioExcelDto> usuarios)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Usuarios");

        worksheet.Cell(1, 1).Value = "ID";
        worksheet.Cell(1, 2).Value = "Nombre";
        worksheet.Cell(1, 3).Value = "Email";
        worksheet.Cell(1, 4).Value = "Fecha de Registro";

        var cabecera = worksheet.Range(1, 1, 1, 4);
        cabecera.Style.Font.Bold = true;
        cabecera.Style.Fill.BackgroundColor = XLColor.LightGray;
        cabecera.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        for (int i = 0; i < usuarios.Count; i++)
        {
            var fila = i + 2;
            worksheet.Cell(fila, 1).Value = usuarios[i].Id;
            worksheet.Cell(fila, 2).Value = usuarios[i].Nombre;
            worksheet.Cell(fila, 3).Value = usuarios[i].Email;
            worksheet.Cell(fila, 4).Value = usuarios[i].FechaRegistro;
        }

        worksheet.Columns().AdjustToContents();

        using var ms = new MemoryStream();
        workbook.SaveAs(ms);
        var bytes = ms.ToArray();

        return new ExcelFile
        {
            FileName = "ReporteUsuarios.xlsx",
            Content = bytes
        };
    }
}