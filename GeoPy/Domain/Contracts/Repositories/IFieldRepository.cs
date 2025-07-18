using Domain.Models;

namespace Domain.Contracts.Repositories;

public interface IFieldRepository
{
    Task<Field?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Field?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<List<Field>> GetAllAsync(CancellationToken cancellationToken = default);
    
    Task<Field> AddAsync(Field field, CancellationToken cancellationToken = default);
    Task UpdateAsync(Field field, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}