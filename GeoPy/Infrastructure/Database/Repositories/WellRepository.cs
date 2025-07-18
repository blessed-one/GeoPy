using Domain.Contracts.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Database.Repositories;

public class WellRepository : IWellRepository
{
    private readonly AppDbContext _context;

    public WellRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Well?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    { 
        return await _context.Wells.FirstOrDefaultAsync(w => w.WellId == id, cancellationToken);
    }
    public async Task<List<Well>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Wells.ToListAsync(cancellationToken);
    }

    public async Task<Well> AddAsync(Well well, CancellationToken cancellationToken = default)
    {
        await _context.Wells.AddAsync(well, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return well;
    }

    public async Task UpdateAsync(Well well, CancellationToken cancellationToken = default)
    {
        _context.Wells.Update(well);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var well = await _context.Wells.FindAsync([id], cancellationToken);
        if (well == null)
            return false;

        _context.Wells.Remove(well);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
