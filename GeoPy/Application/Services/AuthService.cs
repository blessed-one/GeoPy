using Application.Interfaces.Auth;
using Domain.Contracts.Providers;
using Domain.Contracts.Repositories;
using Domain.Contracts.Security;
using Domain.Models;

namespace Application.Services;

public class AuthService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider
) : IAuthService
{
    public async Task<string> LoginAsync(string email, string password)
    {
        var user = await userRepository.GetUserByEmailAsync(email);
        if (user is null)
            throw new UnauthorizedAccessException();

        if (passwordHasher.VerifyPassword(password, user.Password))
            return await jwtProvider.GenerateTokensAsync(user);

        throw new UnauthorizedAccessException();
    }

    public async Task RegisterAsync(string email, string password)
    {
        var newUser = new User
        {
            Email = email,
            Password = passwordHasher.HashPassword(password)
        };
        
        await userRepository.SaveUserAsync(newUser);
    }
}