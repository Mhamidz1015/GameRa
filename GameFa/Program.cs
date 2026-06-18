using GameRa.Extensions;
using GameRa.Modules.Games.Infrastructure;
using GameRa.Modules.Games.Presentation;
using GameRa.Modules.Games.Presentation.Games;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddGamesModule(builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.ApplyMigrations();
}
GameEndpoints.MapEndpoints(app);

app.Run();
