using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Distribution.Application.GameVersions.PublishGameVersion;

public sealed record PublishGameVersionCommand(Guid VersionId) : ICommand;
