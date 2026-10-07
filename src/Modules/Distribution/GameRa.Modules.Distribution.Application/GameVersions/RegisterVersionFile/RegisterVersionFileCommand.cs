using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Distribution.Application.GameVersions.RegisterVersionFile;

public sealed record RegisterVersionFileCommand(
    Guid VersionId,
    string RelativePath,
    long SizeBytes,
    string Sha256)
    : ICommand<RegisterVersionFileResponse>;
