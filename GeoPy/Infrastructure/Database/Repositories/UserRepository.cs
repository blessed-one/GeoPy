using Core.Interfaces.Repositories;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public async Task<User?> GetUserByIdAsync(int userId) =>
        await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);

    public async Task<User?> GetUserByEmailAsync(string email) =>
        await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => string.Equals(u.Email, email));

    public async Task<int> SaveUserAsync(User user)
    {
        if (await dbContext.Users.AnyAsync(u => u.Email == user.Email))
            throw new ArgumentException("User with such email already exists");
        
        var userEntityEntry = await dbContext.Users.AddAsync(user);
        
        return userEntityEntry.Entity.Id;
    }
}