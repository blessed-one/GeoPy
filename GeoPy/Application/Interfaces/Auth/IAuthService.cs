namespace Application.Interfaces.Auth;

public interface IAuthService
{
    Task<string> LoginAsync(string email, string password);
    Task RegisterAsync(string email, string password);
}