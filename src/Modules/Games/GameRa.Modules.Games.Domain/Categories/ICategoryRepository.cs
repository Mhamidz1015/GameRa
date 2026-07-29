namespace GameRa.Modules.Games.Domain.Categories;

public interface ICategoryRepository
{
    Task<Category?> GetAsync(Guid id, CancellationToken cancellationToken = default);

    void Insert(Category category);

    Task<bool> ExistAsync(Guid id, CancellationToken cancellationToken = default);
}
