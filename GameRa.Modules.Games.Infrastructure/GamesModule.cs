using GameRa.Modules.Games.Application.Abstractions.Clock;
using GameRa.Modules.Games.Application.Abstractions.Data;
using GameRa.Modules.Games.Domain.Categories;
using GameRa.Modules.Games.Domain.Games;
using GameRa.Modules.Games.Infrastructure.Games;
using GameRa.Modules.Games.Presentation.Categories;
using GameRa.Modules.Games.Presentation.Games;
using FluentValidation;
using GameRa.Modules.Games.Infrastructure.Categories;
using GameRa.Modules.Games.Infrastructure.Clock;
using GameRa.Modules.Games.Infrastructure.Data;
using GameRa.Modules.Games.Infrastructure.Database;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using static System.Net.Mime.MediaTypeNames;

namespace GameRa.Modules.Games.Infrastructure;

public static class GamesModule
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        CategoryEndpoints.MapEndpoints(app);
        GameEndpoints.MapEndpoints(app);
    }

    public static IServiceCollection AddGamesModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly);
        });

        services.AddValidatorsFromAssembly(Application.AssemblyReference.Assembly, includeInternalTypes: true);

        services.AddInfrastructure(configuration);

        return services;
    }

    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string databaseConnectionString = configuration.GetConnectionString("Database")!;

        NpgsqlDataSource npgsqlDataSource = new NpgsqlDataSourceBuilder(databaseConnectionString).Build();
        services.TryAddSingleton(npgsqlDataSource);

        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

        services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();

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