using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Distribution.Application.GameVersions.MarkVersionReady;

public sealed record MarkVersionReadyCommand(Guid VersionId) : ICommand;
