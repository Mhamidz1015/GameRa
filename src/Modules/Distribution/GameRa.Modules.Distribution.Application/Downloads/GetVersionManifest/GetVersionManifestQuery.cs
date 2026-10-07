using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Distribution.Application.Downloads.GetVersionManifest;

public sealed record GetVersionManifestQuery(Guid UserId, Guid GameId, Guid VersionId) : IQuery<VersionManifestResponse>;
