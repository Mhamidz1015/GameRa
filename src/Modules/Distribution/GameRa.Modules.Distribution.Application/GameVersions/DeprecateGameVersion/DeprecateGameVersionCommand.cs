using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Distribution.Application.GameVersions.DeprecateGameVersion;

public sealed record DeprecateGameVersionCommand(Guid VersionId) : ICommand;
