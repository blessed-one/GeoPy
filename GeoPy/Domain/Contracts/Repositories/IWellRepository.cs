using Domain.Models;

namespace Domain.Contracts.Repositories;

public interface IWellRepository
{
    Task<Well?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<Well>> GetAllAsync(CancellationToken cancellationToken = default);
    
    Task<Well> AddAsync(Well well, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Well well, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}