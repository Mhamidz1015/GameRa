namespace GameRa.Modules.Distribution.Application.Abstractions.Storage;

public interface IFileStorage
{
    Task<PresignedUpload> CreateUploadUrlAsync(
        string storageKey,
        long sizeBytes,
        string sha256,
        TimeSpan ttl,
        CancellationToken cancellationToken = default);

    Task<StoredObjectInfo> GetObjectInfoAsync(string storageKey, CancellationToken cancellationToken = default);

    Task<string> CreateDownloadUrlAsync(string storageKey, TimeSpan ttl, CancellationToken cancellationToken = default);

    Task DeleteAsync(string storageKey, CancellationToken cancellationToken = default);
}
