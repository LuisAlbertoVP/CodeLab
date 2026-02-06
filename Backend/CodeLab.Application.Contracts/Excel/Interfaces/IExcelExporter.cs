using CodeLab.Application.Contracts.Excel.DTOs;

namespace CodeLab.Application.Contracts.Excel.Interfaces;

public interface IExcelExporter<T>
{
    ExcelFile ExportToExcel(T data);
}