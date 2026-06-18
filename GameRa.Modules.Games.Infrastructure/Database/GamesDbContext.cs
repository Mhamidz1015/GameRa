using GameRa.Modules.Games.Application.Abstractions.Data;
using GameRa.Modules.Games.Domain.Games;
using Microsoft.EntityFrameworkCore;

namespace GameRa.Modules.Games.Infrastructure.Database;

public sealed class GamesDbContext(DbContextOptions<GamesDbContext> options) : DbContext(options), IUnitOfWork
{
    internal DbSet<Game> Games { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Games);
    }
}
