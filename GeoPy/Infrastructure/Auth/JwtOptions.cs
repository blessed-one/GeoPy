namespace Infrastructure.Auth;

public class JwtOptions
{
    public required string SecretKey { get; set; }
    public int ExpireHours { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
}