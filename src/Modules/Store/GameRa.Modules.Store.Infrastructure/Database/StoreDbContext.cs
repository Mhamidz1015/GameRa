using GameRa.Common.Application.Data;
using GameRa.Common.Infrastructure.Inbox;
using GameRa.Common.Infrastructure.Outbox;
using GameRa.Modules.Store.Application.Abstractions.Data;
using GameRa.Modules.Store.Domain.Customers;
using GameRa.Modules.Store.Domain.Games;
using GameRa.Modules.Store.Domain.Orders;
using GameRa.Modules.Store.Domain.Payments;
using GameRa.Modules.Store.Domain.Wishlist;
using GameRa.Modules.Store.Infrastructure.Customers;
using GameRa.Modules.Store.Infrastructure.Games;
using GameRa.Modules.Store.Infrastructure.Orders;
using GameRa.Modules.Store.Infrastructure.Payments;
using GameRa.Modules.Store.Infrastructure.Wishlists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;

namespace GameRa.Modules.Store.Infrastructure.Database;

public sealed class StoreDbContext(DbContextOptions<StoreDbContext> options)
    : DbContext(options), IUnitOfWork
{
    internal DbSet<Customer> Customers { get; set; }

    internal DbSet<Game> Games { get; set; }

    internal DbSet<Order> Orders { get; set; }

    internal DbSet<OrderItem> OrderItems { get; set; }

    internal DbSet<Payment> Payments { get; set; }
    internal DbSet<WishlistItem> WishlistItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Store);

        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        modelBuilder.ApplyConfiguration(new GameConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageConsumerConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageConsumerConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentConfiguration());
        modelBuilder.ApplyConfiguration(new WishlistItemConfiguration());
    }

    public async Task<DbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Database.CurrentTransaction is not null)
        {
            await Database.CurrentTransaction.DisposeAsync();
        }

        return new EfCoreDbTransaction(await Database.BeginTransactionAsync(cancellationToken));
    }
}
