namespace CodeLab.Application.DTOs.Excel;

public class ExcelFile
{
    public string ContentType { get; } = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    public string FileName { get; set; }
    
    public byte[] Content { get; set; }
}