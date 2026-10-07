namespace GameRa.Modules.Distribution.Infrastructure.Storage;

internal sealed class StorageOptions
{
    public string ServiceUrl { get; init; } = string.Empty;

    public string Region { get; init; } = "us-east-1";

    public string AccessKey { get; init; } = string.Empty;

    public string SecretKey { get; init; } = string.Empty;

    public string BucketName { get; init; } = string.Empty;

    public bool ForcePathStyle { get; init; } = true;
}
