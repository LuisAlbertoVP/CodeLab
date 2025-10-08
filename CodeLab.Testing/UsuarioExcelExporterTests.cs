using Bogus;
using ClosedXML.Excel;
using CodeLab.Infrastructure.Excel.Contracts.DTOs;
using CodeLab.Infrastructure.Excel.Services;
using FluentAssertions;

namespace CodeLab.Testing;

public class UsuarioExcelExporterTests
{
    [Fact]
    public void GenerarReporte_DebeCrearExcelConUsuariosFalsos()
    {
        var exporter = new UsuarioExcelExporter();

        var faker = new Faker<ReporteUsuarioExcelDto>()
            .RuleFor(u => u.Id, f => f.IndexFaker + 1)
            .RuleFor(u => u.Nombre, f => f.Name.FullName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.FechaRegistro, f => f.Date.Past(1));

        var usuarios = faker.Generate(5);

        var excelFile = exporter.ExportToExcel(usuarios);

        excelFile.Should().NotBeNull();
        excelFile.Content.Length.Should().BeGreaterThan(0);

        using var stream = new MemoryStream(excelFile.Content);
        using var workbook = new XLWorkbook(stream);
        workbook.Worksheets.Should().HaveCount(1);

        var worksheet = workbook.Worksheet("Usuarios");

        worksheet.Cell(1, 1).GetString().Should().Be("ID");
        worksheet.Cell(1, 2).GetString().Should().Be("Nombre");
        worksheet.Cell(1, 3).GetString().Should().Be("Email");
        worksheet.Cell(1, 4).GetString().Should().Be("Fecha de Registro");

        worksheet.Cell(2, 2).GetString().Should().Be(usuarios[0].Nombre);
        worksheet.Cell(2, 3).GetString().Should().Be(usuarios[0].Email);
    }
}