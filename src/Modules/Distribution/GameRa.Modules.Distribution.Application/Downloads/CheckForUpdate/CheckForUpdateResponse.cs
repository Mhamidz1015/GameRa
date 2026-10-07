namespace GameRa.Modules.Distribution.Application.Downloads.CheckForUpdate;

public sealed record CheckForUpdateResponse(
    bool UpdateAvailable,
    string CurrentVersion,
    string LatestVersion,
    Guid LatestVersionId);
