using Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddPostgresDb(configuration);
services.AddAuth(configuration);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
