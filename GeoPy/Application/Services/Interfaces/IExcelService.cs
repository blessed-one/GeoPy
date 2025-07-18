using Application.DTOs;

namespace Application.Services.Interfaces;

public interface IExcelService
{
    Task<ImportResult> ImportFromExcelAsync(Stream stream);
    Task<byte[]> ExportToExcelAsync();

}