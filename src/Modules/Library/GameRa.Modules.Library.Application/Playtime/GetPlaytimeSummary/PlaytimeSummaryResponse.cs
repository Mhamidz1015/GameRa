namespace GameRa.Modules.Library.Application.Playtime.GetPlaytimeSummary;

public sealed class PlaytimeSummaryResponse
{
    public Guid GameId { get; init; }
    public int TotalMinutes { get; init; }
    public DateTime LastPlayedUtc { get; init; }
}