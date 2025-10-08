using CodeLab.Infrastructure.Excel.Contracts.DTOs;

namespace CodeLab.Infrastructure.Excel.Contracts.Interfaces;

public interface IExcelExporter<T>
{
    ExcelFile ExportToExcel(T data);
}