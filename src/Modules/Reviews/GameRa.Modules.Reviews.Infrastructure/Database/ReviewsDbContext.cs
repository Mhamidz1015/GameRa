using GameRa.Common.Infrastructure.Inbox;
using GameRa.Common.Infrastructure.Outbox;
using GameRa.Modules.Reviews.Application.Abstractions.Data;
using GameRa.Modules.Reviews.Domain;
using GameRa.Modules.Reviews.Infrastructure.Reviews;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace GameRa.Modules.Reviews.Infrastructure.Database;

public sealed class ReviewsDbContext(DbContextOptions<ReviewsDbContext> options)
    : DbContext(options), IUnitOfWork
{
    internal DbSet<Review> Reviews { get; set; }
    internal DbSet<VerifiedPurchase> VerifiedPurchases { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Reviews);

        modelBuilder.ApplyConfiguration(new ReviewConfiguration());
        modelBuilder.ApplyConfiguration(new VerifiedPurchaseConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageConsumerConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageConsumerConfiguration());
    }
}
