using GameRa.Modules.Games.Infrastructure.Games;
using GameRa.Modules.Games.Domain.Categories;
using GameRa.Modules.Games.Domain.Games;
using Microsoft.EntityFrameworkCore;
using GameRa.Common.Application.Data;

namespace GameRa.Modules.Games.Infrastructure.Database;

public sealed class GamesDbContext(DbContextOptions<GamesDbContext> options) : DbContext(options), IUnitOfWork
{
    internal DbSet<Game> Games { get; set; }
    internal DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Games);

        modelBuilder.ApplyConfiguration(new GameConfiguration());
    }
}
