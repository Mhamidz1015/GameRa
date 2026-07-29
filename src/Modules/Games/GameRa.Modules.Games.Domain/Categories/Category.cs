using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Games.Domain.Categories;

public sealed class Category : Entity
{
    private Category()
    {
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public bool IsArchived { get; private set; }

    public static Result<Category> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Category>(CategoryErrors.NameIsEmpty);

        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = name,
            IsArchived = false
        };

        category.Raise(new CategoryCreatedDomainEvent(category.Id));

        return Result.Success(category);
    }

    public void Archive()
    {
        IsArchived = true;

        Raise(new CategoryArchivedDomainEvent(Id));
    }

    public void ChangeName(string name)
    {
        if (Name == name)
        {
            return;
        }

        Name = name;

        Raise(new CategoryNameChangedDomainEvent(Id, Name));
    }
}
