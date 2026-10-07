using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Distribution.Application.GameVersions.CreateGameVersion;

public sealed record CreateGameVersionCommand(Guid GameId, string Version, string? ReleaseNotes) : ICommand<Guid>;
