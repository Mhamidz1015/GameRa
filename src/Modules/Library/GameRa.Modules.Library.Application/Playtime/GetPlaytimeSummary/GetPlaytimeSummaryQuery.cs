using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Library.Application.Playtime.GetPlaytimeSummary;

public sealed record GetPlaytimeSummaryQuery(Guid UserId)
    : IQuery<IReadOnlyCollection<PlaytimeSummaryResponse>>;
