using Domain.Models;

namespace Domain.Contracts.Providers;

public interface IJwtProvider
{
    Task<string> GenerateTokensAsync(User user);
}