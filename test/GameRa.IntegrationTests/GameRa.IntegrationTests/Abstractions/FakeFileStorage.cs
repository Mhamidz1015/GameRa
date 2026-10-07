using System.Collections.Concurrent;
using GameRa.Modules.Distribution.Application.Abstractions.Storage;

namespace GameRa.IntegrationTests.Abstractions;

public sealed class FakeFileStorage : IFileStorage
{
    private readonly ConcurrentDictionary<string, StoredObjectInfo> _objects = new();

    public void SimulateUpload(string storageKey, long sizeBytes, string sha256)
    {
        _objects[storageKey] = new StoredObjectInfo(true, sizeBytes, sha256);
    }

    public bool Contains(string storageKey) => _objects.ContainsKey(storageKey);

    public Task<PresignedUpload> CreateUploadUrlAsync(
        string storageKey,
        long sizeBytes,
        string sha256,
        TimeSpan ttl,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new PresignedUpload(
            $"https://fake-storage.local/upload/{storageKey}",
            new Dictionary<string, string> { ["x-amz-checksum-sha256"] = sha256 }));
    }

    public Task<StoredObjectInfo> GetObjectInfoAsync(string storageKey, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_objects.TryGetValue(storageKey, out StoredObjectInfo? info) ? info : StoredObjectInfo.Missing);
    }

    public Task<string> CreateDownloadUrlAsync(string storageKey, TimeSpan ttl, CancellationToken cancellationToken = default)
    {
        return Task.FromResult($"https://fake-storage.local/download/{storageKey}?ttl={(int)ttl.TotalSeconds}");
    }

    public Task DeleteAsync(string storageKey, CancellationToken cancellationToken = default)
    {
        _objects.TryRemove(storageKey, out _);
        return Task.CompletedTask;
    }
}
