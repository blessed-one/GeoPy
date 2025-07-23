using System.Text;
using Application.Interfaces.Auth;
using Application.Services;
using Application.Services.Interfaces;
using Domain.Contracts.Providers;
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
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IWellService, WellService>();

        return services;
    }
}