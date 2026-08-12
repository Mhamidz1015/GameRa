using GameRa.Modules.Discounts.Infrastructure.Database;
using GameRa.Modules.Games.Infrastructure.Database;
using GameRa.Modules.Library.Infrastructure.Database;
using GameRa.Modules.Reviews.Infrastructure.Database;
using GameRa.Modules.Store.Infrastructure.Database;
using GameRa.Modules.Users.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GameRa.Extensions;

internal static class MigrationExtensions
{
    internal static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        ApplyMigration<GamesDbContext>(scope);
        ApplyMigration<UsersDbContext>(scope);
        ApplyMigration<LibraryItemDbContext>(scope);
        ApplyMigration<StoreDbContext>(scope);
        ApplyMigration<DiscountDbContext>(scope);
        ApplyMigration<ReviewsDbContext>(scope);
    }

    private static void ApplyMigration<TDbContext>(IServiceScope scope)
    where TDbContext : DbContext
    {
        using TDbContext context = scope.ServiceProvider.GetRequiredService<TDbContext>();

        try
        {
            context.Database.Migrate();
        }
        catch (Exception)
        {
            // Migration already applied — ignore duplicate key errors
        }
    }
}
