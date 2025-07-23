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
        Console.WriteLine(new string('-', 50));
        Console.WriteLine(new string('-', 50));
        Console.WriteLine(well.WellId);
        //well.WellId = 0;
        var loadedWell= await _context.Wells.FindAsync([well.WellId], cancellationToken: cancellationToken);
        if (loadedWell is not null)
        {
            throw new Exception("Запись о сущности Well уже существует");
        }
        
        await _context.Wells.AddAsync(well, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return well;
    }

    public async Task<bool> UpdateAsync(Well well, CancellationToken cancellationToken = default)
    {
        var loadedWell = await _context.Wells.FindAsync([well.WellId], cancellationToken: cancellationToken);
        if (loadedWell is null)
        {
            throw new Exception("Не удалось найти запись о сущности Well");
        }
        
        var entry = _context.Entry(loadedWell);
        var initialValues = entry.CurrentValues.Clone();
    
        entry.CurrentValues.SetValues(well);
    
        var hasChanges = !entry.CurrentValues.Properties
            .All(property => 
                Equals(initialValues[property], entry.CurrentValues[property]));

        if (!hasChanges) 
            return false;
        
        await _context.SaveChangesAsync(cancellationToken);
        return true; 
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
