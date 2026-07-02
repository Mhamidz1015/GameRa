using GameRa.Modules.Games.Domain.Categories;
using GameRa.Modules.Games.Domain.Games;
using GameRa.Modules.Games.Infrastructure.Games;
using GameRa.Modules.Games.Infrastructure.Categories;
using GameRa.Modules.Games.Infrastructure.Database;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GameRa.Common.Application.Data;
using GameRa.Common.Application.Clock;
using GameRa.Common.Presentation.Endpoints;

namespace GameRa.Modules.Games.Infrastructure;

public static class GamesModule
{

    public static IServiceCollection AddGamesModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        services.AddInfrastructure(configuration);

        return services;
    }

    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string databaseConnectionString = configuration.GetConnectionString("Database")!;

        services.AddDbContext<GamesDbContext>(options =>
            options
                .UseNpgsql(
                    databaseConnectionString,
                    npgsqlOptions => npgsqlOptions
                        .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Games))
                .UseSnakeCaseNamingConvention()
                .AddInterceptors());

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<GamesDbContext>());

        services.AddScoped<IGameRepository, GameRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
    }
}