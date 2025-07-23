using API.Filters;
using Infrastructure.Mappings;
using Microsoft.OpenApi.Models;

namespace API.Configurations;

public static class ServicesCollectionExtensions
{
    public static void ConfigureAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<API.Mappings.MappingProfile>();
            cfg.AddProfile<MappingProfile>();
            cfg.AddProfile<Application.Mappings.MappingProfile>();
        });
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "GeoPy API",
                Version = "v1",
                Description = "API для CRUD операций со скважинами"
            });
            
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT токен в заголовке Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
            
            c.OperationFilter<WellsOperationsFilter>();
        });
    }
}