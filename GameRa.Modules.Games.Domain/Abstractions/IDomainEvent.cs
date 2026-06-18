namespace GameRa.Modules.Games.Domain.Abstractions;

public interface IDomainEvent
{
    Guid Id { get; }

    DateTime OccurredOnUtc { get; }
}
