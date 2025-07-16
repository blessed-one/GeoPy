using Core.Models;

namespace Core.Interfaces.Auth;

public interface IJwtProvider
{
    Task<string> GenerateTokensAsync(User user);
}