using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Distribution.Application.GameVersions.RemoveVersionFile;

public sealed record RemoveVersionFileCommand(Guid VersionId, Guid FileId) : ICommand;
