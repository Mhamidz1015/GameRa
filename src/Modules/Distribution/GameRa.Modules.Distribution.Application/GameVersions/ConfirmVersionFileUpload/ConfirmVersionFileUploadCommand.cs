using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Distribution.Application.GameVersions.ConfirmVersionFileUpload;

public sealed record ConfirmVersionFileUploadCommand(Guid VersionId, Guid FileId) : ICommand;
