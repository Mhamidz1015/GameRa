using GameRa.Modules.Games.Domain.Categories;
using GameRa.Modules.Games.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GameRa.Modules.Games.Infrastructure.Categories;

internal sealed class CategoryRepository(GamesDbContext context) : ICategoryRepository
{
    public async Task<bool> ExistAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Categories.AnyAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Category?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Categories.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public void Insert(Category category)
    {
        context.Categories.Add(category);
    }
}
