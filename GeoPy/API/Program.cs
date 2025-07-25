using API;
using API.Configurations;
using Application;
using Infrastructure;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

builder.Services.AddControllers();

services.ConfigureAutoMapper();
services.ConfigureSwagger();

services.AddAuth(configuration);

services
    .AddApplication(configuration)
    .AddInfrastructure(configuration);

services.AddCors(options => options.AddDefaultPolicy(policyBuilder =>
{
    var origins = builder.Configuration.GetSection("CorsPolicy:Origins").Get<string[]>()
                  ?? throw new ApplicationException("CorsPolicy is not configured");
    policyBuilder
        .WithOrigins(origins)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
}));

var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    MigrateDatabase(app);
}


app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
return;

static void MigrateDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    using var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (db.Database.GetPendingMigrations().Any())
    {
        db.Database.Migrate();
    }
}