using GameRa.Common.Infrastructure.Inbox;
using GameRa.Common.Infrastructure.Outbox;
using GameRa.Modules.Distribution.Application.Abstractions.Data;
using GameRa.Modules.Distribution.Domain.Games;
using GameRa.Modules.Distribution.Domain.GameVersions;
using GameRa.Modules.Distribution.Domain.Ownerships;
using GameRa.Modules.Distribution.Infrastructure.Games;
using GameRa.Modules.Distribution.Infrastructure.GameVersions;
using GameRa.Modules.Distribution.Infrastructure.Ownerships;
using Microsoft.EntityFrameworkCore;

namespace GameRa.Modules.Distribution.Infrastructure.Database;

public sealed class DistributionDbContext(DbContextOptions<DistributionDbContext> options)
    : DbContext(options), IUnitOfWork
{
    internal DbSet<GameVersion> GameVersions { get; set; }

    internal DbSet<VersionFile> VersionFiles { get; set; }

    internal DbSet<GameOwnership> GameOwnerships { get; set; }

    internal DbSet<DistributionGame> Games { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Distribution);

        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageConsumerConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageConsumerConfiguration());
        modelBuilder.ApplyConfiguration(new GameVersionConfiguration());
        modelBuilder.ApplyConfiguration(new VersionFileConfiguration());
        modelBuilder.ApplyConfiguration(new GameOwnershipConfiguration());
        modelBuilder.ApplyConfiguration(new DistributionGameConfiguration());
    }
}
