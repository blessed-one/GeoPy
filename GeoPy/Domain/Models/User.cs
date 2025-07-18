namespace Domain.Models;

public class User
{
    public int Id { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}