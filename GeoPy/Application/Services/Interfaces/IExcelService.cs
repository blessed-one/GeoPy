using Application.DTOs;

namespace Application.Services.Interfaces;

public interface IExcelService
{
    Task<ExcelImportResult> ImportFromExcelAsync(Stream stream);
    Task<byte[]> ExportToExcelAsync();

}