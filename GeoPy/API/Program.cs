using API;
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

//services.AddAuth(configuration);

services
    .AddApplication(configuration)
    .AddInfrastructure(configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    MigrateDatabase(app);
}

app.MapControllers();

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