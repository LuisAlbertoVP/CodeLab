using CodeLab.Application.DTOs.Excel;

namespace CodeLab.Application.Interfaces.Excel;

public interface IExcelExporter<T>
{
    ExcelFile ExportToExcel(T data);
}