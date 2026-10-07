using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Distribution.Domain.Ownerships;

public sealed class GameOwnership : Entity
{
    private GameOwnership()
    {
    }

    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public Guid GameId { get; private set; }

    public bool IsOwned { get; private set; }

    public DateTime LastEventOnUtc { get; private set; }

    public static GameOwnership Create(Guid userId, Guid gameId, bool owned, DateTime occurredOnUtc)
    {
        return new GameOwnership
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            GameId = gameId,
            IsOwned = owned,
            LastEventOnUtc = ToStoragePrecision(occurredOnUtc)
        };
    }

    public bool Apply(bool owned, DateTime occurredOnUtc)
    {
        DateTime eventTime = ToStoragePrecision(occurredOnUtc);

        // Only a strictly newer event wins, so duplicated and out-of-order events are ignored.
        if (eventTime <= LastEventOnUtc)
        {
            return false;
        }

        IsOwned = owned;
        LastEventOnUtc = eventTime;

        return true;
    }

    // PostgreSQL keeps microseconds; comparing at that precision keeps duplicates idempotent after a round trip.
    private static DateTime ToStoragePrecision(DateTime value) =>
        new(value.Ticks - value.Ticks % 10, DateTimeKind.Utc);
}
