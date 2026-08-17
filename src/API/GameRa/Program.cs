using GameRa.Common.Application;
using GameRa.Common.Infrastructure;
using GameRa.Common.Infrastructure.Configuration;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Extensions;
using GameRa.Middleware;
using GameRa.Modules.Discounts.Infrastructure;
using GameRa.Modules.Games.Infrastructure;
using GameRa.Modules.Library.Infrastructure;
using GameRa.Modules.Reviews.Infrastructure;
using GameRa.Modules.Store.Infrastructure;
using GameRa.Modules.Users.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();

Assembly[] moduleApplicationAssemblies = [
    GameRa.Modules.Users.Application.AssemblyReference.Assembly,
    GameRa.Modules.Games.Application.AssemblyReference.Assembly,
    GameRa.Modules.Store.Application.AssemblyReference.Assembly,
    GameRa.Modules.Library.Application.AssemblyReference.Assembly,
    GameRa.Modules.Reviews.Application.AssemblyReference.Assembly,
    GameRa.Modules.Discounts.Application.AssemblyReference.Assembly];

builder.Services.AddApplication(moduleApplicationAssemblies);

string databaseConnectionString = builder.Configuration.GetConnectionStringOrThrow("Database");
string redisConnectionString = builder.Configuration.GetConnectionStringOrThrow("Cache");

builder.Services.AddInfrastructure(
    [
        StoreModule.ConfigureConsumers,
        LibraryItemModule.ConfigureConsumers,
        ReviewsModule.ConfigureConsumers
    ],
    databaseConnectionString,
    redisConnectionString);

Uri keyCloakHealthUrl = builder.Configuration.GetKeyCloakHealthUrl();

builder.Services.AddHealthChecks()
    .AddNpgSql(databaseConnectionString)
    .AddRedis(redisConnectionString)
    .AddKeyCloak(keyCloakHealthUrl);

builder.Configuration.AddModuleConfiguration(
    ["users", "games", "store" , "LibraryItem" , "Reviews" , "Discounts"]);

builder.Services.AddGamesModule(builder.Configuration);

builder.Services.AddUsersModule(builder.Configuration);

builder.Services.AddStoreModule(builder.Configuration);

builder.Services.AddLibraryItemModule(builder.Configuration);

builder.Services.AddReviewsModule(builder.Configuration);

builder.Services.AddDiscountsModule(builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
    
}

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapEndpoints();

app.Run();

public partial class Program;
