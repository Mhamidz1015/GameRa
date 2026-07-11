using GameRa.Common.Application.Data;
using GameRa.Common.Infrastructure.Inbox;
using GameRa.Common.Infrastructure.Outbox;
using GameRa.Modules.Library.Application.Abstractions.Data;
using GameRa.Modules.Library.Domain.LibraryItems;
using GameRa.Modules.Library.Infrastructure.LibraryItems;
using Microsoft.EntityFrameworkCore;

namespace GameRa.Modules.Library.Infrastructure.Database;

public sealed class LibraryItemDbContext(DbContextOptions<LibraryItemDbContext> options)
    : DbContext(options), IUnitOfWork
{
    internal DbSet<LibraryItem> LibraryItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.LibraryItem);

        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageConsumerConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageConsumerConfiguration());
        modelBuilder.ApplyConfiguration(new LibraryItemConfiguration());
    }
}
