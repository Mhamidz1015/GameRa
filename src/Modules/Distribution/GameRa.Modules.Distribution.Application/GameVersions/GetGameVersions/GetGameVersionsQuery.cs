using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Distribution.Application.GameVersions.GetGameVersions;

public sealed record GetGameVersionsQuery(Guid GameId) : IQuery<IReadOnlyCollection<GameVersionResponse>>;
