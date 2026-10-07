using System.Net;
using System.Security.Cryptography;
using Amazon.S3;
using Amazon.S3.Model;
using GameRa.Modules.Distribution.Application.Abstractions.Storage;
using Microsoft.Extensions.Options;

namespace GameRa.Modules.Distribution.Infrastructure.Storage;

internal sealed class S3FileStorage(IAmazonS3 s3, IOptions<StorageOptions> options) : IFileStorage
{
    private const string ChecksumHeader = "x-amz-checksum-sha256";

    private readonly StorageOptions _options = options.Value;

    private Protocol Protocol =>
        _options.ServiceUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ? Protocol.HTTP : Protocol.HTTPS;

    public async Task<PresignedUpload> CreateUploadUrlAsync(
        string storageKey,
        long sizeBytes,
        string sha256,
        TimeSpan ttl,
        CancellationToken cancellationToken = default)
    {
        string checksum = Convert.ToBase64String(Convert.FromHexString(sha256));

        var request = new GetPreSignedUrlRequest
        {
            BucketName = _options.BucketName,
            Key = storageKey,
            Verb = HttpVerb.PUT,
            Expires = DateTime.UtcNow.Add(ttl),
            Protocol = Protocol
        };

        // The checksum header is part of the signature, so the storage rejects any body that does not match it.
        request.Headers[ChecksumHeader] = checksum;

        string url = await s3.GetPreSignedURLAsync(request);

        return new PresignedUpload(url, new Dictionary<string, string> { [ChecksumHeader] = checksum });
    }

    public async Task<StoredObjectInfo> GetObjectInfoAsync(
        string storageKey,
        CancellationToken cancellationToken = default)
    {
        GetObjectMetadataResponse metadata;
        try
        {
            metadata = await s3.GetObjectMetadataAsync(
                new GetObjectMetadataRequest
                {
                    BucketName = _options.BucketName,
                    Key = storageKey,
                    ChecksumMode = ChecksumMode.ENABLED
                },
                cancellationToken);
        }
        catch (AmazonS3Exception exception) when (exception.StatusCode == HttpStatusCode.NotFound)
        {
            return StoredObjectInfo.Missing;
        }

        string? storedChecksum = metadata.ChecksumSHA256;

        // Objects written without a full-object SHA-256 checksum are hashed by streaming them (approved fallback).
        string sha256 = string.IsNullOrEmpty(storedChecksum) || storedChecksum.Contains('-')
            ? await ComputeSha256Async(storageKey, cancellationToken)
            : Convert.ToHexStringLower(Convert.FromBase64String(storedChecksum));

        return new StoredObjectInfo(true, metadata.ContentLength, sha256);
    }

    public async Task<string> CreateDownloadUrlAsync(
        string storageKey,
        TimeSpan ttl,
        CancellationToken cancellationToken = default)
    {
        return await s3.GetPreSignedURLAsync(new GetPreSignedUrlRequest
        {
            BucketName = _options.BucketName,
            Key = storageKey,
            Verb = HttpVerb.GET,
            Expires = DateTime.UtcNow.Add(ttl),
            Protocol = Protocol
        });
    }

    public async Task DeleteAsync(string storageKey, CancellationToken cancellationToken = default)
    {
        await s3.DeleteObjectAsync(_options.BucketName, storageKey, cancellationToken);
    }

    private async Task<string> ComputeSha256Async(string storageKey, CancellationToken cancellationToken)
    {
        using GetObjectResponse response = await s3.GetObjectAsync(_options.BucketName, storageKey, cancellationToken);

        byte[] hash = await SHA256.HashDataAsync(response.ResponseStream, cancellationToken);

        return Convert.ToHexStringLower(hash);
    }
}
