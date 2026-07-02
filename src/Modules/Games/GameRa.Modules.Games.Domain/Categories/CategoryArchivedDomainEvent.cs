using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Games.Domain.Categories;

public sealed class CategoryArchivedDomainEvent(Guid categoryId) : DomainEvent
{
    public Guid CategoryId { get; init; } = categoryId;
}
