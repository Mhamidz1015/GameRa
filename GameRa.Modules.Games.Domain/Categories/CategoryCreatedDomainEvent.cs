using GameRa.Modules.Games.Domain.Abstractions;

namespace GameRa.Modules.Games.Domain.Categories;

public sealed class CategoryCreatedDomainEvent(Guid categoryId) : DomainEvent
{
    public Guid CategoryId { get; init; } = categoryId;
}
