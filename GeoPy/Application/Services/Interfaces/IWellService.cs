using Application.DTOs;

namespace Application.Services.Interfaces;

public interface IWellService
{
    Task<WellDto?> GetWellAsync(int id);
    Task<IEnumerable<WellDto>> GetAllWellsAsync();
    Task<WellDto> CreateWellAsync(WellDto dto);
    Task UpdateWellAsync(WellDto dto);
    Task DeleteWellAsync(int id);
}