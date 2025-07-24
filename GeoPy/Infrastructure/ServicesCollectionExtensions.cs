using System.Text;
using Application.Services.Interfaces;
using Domain.Contracts.Providers;
using Domain.Contracts.Repositories;
using Domain.Contracts.Security;
using Infrastructure.Auth;
using Infrastructure.Database;
using Infrastructure.Database.Repositories;
using Infrastructure.Security;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public static class ServicesCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddPostgresDb(configuration);
        
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtProvider, JwtProvider>();

        services.AddScoped<IExcelService, ExcelService>();
        
        services.AddScoped<IWellRepository, WellRepository>();
        services.AddScoped<IFieldRepository, FieldRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        
        return services;
    }

    private static IServiceCollection AddPostgresDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString(nameof(AppDbContext)));
            options.UseSnakeCaseNamingConvention();
        });
        
        return services;
    }
    
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
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