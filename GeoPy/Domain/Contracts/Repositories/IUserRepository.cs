using Domain.Models;

namespace Domain.Contracts.Repositories;

public interface IUserRepository
{
    public Task<User?> GetUserByIdAsync(int userId);

    public Task<User?> GetUserByEmailAsync(string email);

    public Task<int> SaveUserAsync(User user);
}