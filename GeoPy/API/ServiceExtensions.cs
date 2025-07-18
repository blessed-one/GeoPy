using Application;

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
}