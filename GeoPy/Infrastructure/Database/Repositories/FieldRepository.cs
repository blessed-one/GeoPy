using Domain.Contracts.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Database.Repositories;

public class FieldRepository : IFieldRepository
{
    private readonly AppDbContext _context;

    public FieldRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Field?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Fields.FindAsync([id], cancellationToken);
    }
    
    public async Task<Field?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Fields.FirstOrDefaultAsync(f => f.FieldName == name, cancellationToken);
    }
    public async Task<List<Field>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Fields.ToListAsync(cancellationToken);
    }

    public async Task<Field> AddAsync(Field field, CancellationToken cancellationToken = default)
    {
        await _context.Fields.AddAsync(field, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return field;
    }

    public async Task UpdateAsync(Field field, CancellationToken cancellationToken = default)
    {
        _context.Fields.Update(field);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var field = await _context.Fields.FindAsync([id], cancellationToken);
        if (field == null)
            return false;

        _context.Fields.Remove(field);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
