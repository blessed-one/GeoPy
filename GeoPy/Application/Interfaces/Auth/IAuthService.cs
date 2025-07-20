namespace Core.Interfaces.Auth;

public interface IAuthService
{
    Task<string> Login(string email, string password);
    Task Register(string email, string password);
}