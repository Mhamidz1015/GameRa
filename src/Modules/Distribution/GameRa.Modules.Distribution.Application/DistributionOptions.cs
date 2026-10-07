namespace GameRa.Modules.Distribution.Application;

public sealed class DistributionOptions
{
    public long MaxFileSizeBytes { get; init; } = 5L * 1024 * 1024 * 1024;

    public TimeSpan UploadUrlTtl { get; init; } = TimeSpan.FromHours(1);

    public TimeSpan DownloadUrlTtl { get; init; } = TimeSpan.FromMinutes(15);

    public int MaxBatchDownloadUrls { get; init; } = 200;
}
