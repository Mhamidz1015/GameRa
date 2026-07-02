using GameRa.Common.Application;
using GameRa.Common.Infrastructure;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Extensions;
using GameRa.Middleware;
using GameRa.Modules.Games.Infrastructure;
using GameRa.Modules.Users.Infrastructure;
using GameRa.Modules.Store.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, LoggerConfig) => LoggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(t => t.FullName?.Replace("+", "."));
});
builder.Services.AddApplication([
    GameRa.Modules.Games.Application.AssemblyReference.Assembly,
    GameRa.Modules.Users.Application.AssemblyReference.Assembly,
    GameRa.Modules.Store.Application.AssemblyReference.Assembly]);

string databaseConnectionString = builder.Configuration.GetConnectionString("Database")!;
string redisConnectionString = builder.Configuration.GetConnectionString("Cache")!;

builder.Services.AddInfrastructure(
    [StoreModule.ConfigureConsumers],
    databaseConnectionString,
    redisConnectionString);

builder.Configuration.AddModuleConfiguration(["games" , "user" , "store"]);

builder.Services.AddHealthChecks()
    .AddNpgSql(databaseConnectionString)
    .AddRedis(redisConnectionString)
    .AddUrlGroup(new Uri(builder.Configuration.GetValue<string>("KeyCloak:HealthUrl")!), HttpMethod.Get, "keycloak");

builder.Services.AddGamesModule(builder.Configuration);
builder.Services.AddUsersModule(builder.Configuration);
builder.Services.AddStoreModule(builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

app.MapEndpoints();

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.Run();
