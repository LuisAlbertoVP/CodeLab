namespace CodeLab.Application.Contracts.Excel.DTOs;

public class ExcelFile
{
    public string ContentType { get; } = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    public string FileName { get; set; }
    
    public byte[] Content { get; set; }
}