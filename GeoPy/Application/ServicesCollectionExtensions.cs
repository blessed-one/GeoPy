using System.Text;
using Application.Services;
using Application.Services.Interfaces;
using Core.Interfaces.Auth;
using Domain.Contracts.Providers;
using Domain.Contracts.Security;
using Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Application;

public static class ServicesCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IWellService, WellService>();
        services.AddScoped<IExcelService, ExcelService>();
        
        return services;
    }
    
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(nameof(JwtOptions)))
            .Validate(options => !string.IsNullOrEmpty(options.SecretKey),
                "JWT SecretKey is required")
            .Validate(options => !string.IsNullOrEmpty(options.Issuer),
                "JWT Issuer is required")
            .Validate(options => !string.IsNullOrEmpty(options.Audience),
                "JWT Audience is required")
            .ValidateOnStart();

        var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>()
                         ?? throw new ApplicationException("JWT is not configured");

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                };
            });

        services.AddAuthorization();
        
        return services;
    }
}