using API.Filters;
using Application;
using Microsoft.OpenApi.Models;

namespace API;

public static class ServiceExtensions
{
    public static void ConfigureAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<ApiMappingProfile>();
            cfg.AddProfile<ApplicationMappingProfile>();
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
            
            c.OperationFilter<WellsOperationsFilter>();
        });
    }
}