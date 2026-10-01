namespace GameRa.Modules.Library.Domain.LibraryItems;

public sealed class PlaytimeRecord
{
    private PlaytimeRecord() { }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid GameId { get; private set; }
    public int TotalMinutes { get; private set; }
    public DateTime LastPlayedUtc { get; private set; }

    public static PlaytimeRecord Create(Guid userId, Guid gameId)
    {
        return new PlaytimeRecord
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            GameId = gameId,
            TotalMinutes = 0,
            LastPlayedUtc = DateTime.UtcNow
        };
    }

    public void AddPlaytime(int minutes)
    {
        TotalMinutes += minutes;
        LastPlayedUtc = DateTime.UtcNow;
    }
}