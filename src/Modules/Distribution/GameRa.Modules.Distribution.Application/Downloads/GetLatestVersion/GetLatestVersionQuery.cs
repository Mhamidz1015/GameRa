using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Distribution.Application.Downloads.GetLatestVersion;

public sealed record GetLatestVersionQuery(Guid UserId, Guid GameId) : IQuery<LatestVersionResponse>;
