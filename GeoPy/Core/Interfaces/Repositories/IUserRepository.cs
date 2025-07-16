using Core.Models;

namespace Core.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(int userId);
    Task<User?> GetUserByEmailAsync(string email);
    Task<int> SaveUserAsync(User user);
}