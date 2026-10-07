namespace GameRa.Modules.Distribution.Application.GameVersions.RegisterVersionFile;

public sealed record RegisterVersionFileResponse(
    Guid FileId,
    string UploadUrl,
    IReadOnlyDictionary<string, string> UploadHeaders,
    DateTime ExpiresAtUtc);
