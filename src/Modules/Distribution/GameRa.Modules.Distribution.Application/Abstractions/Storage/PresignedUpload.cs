namespace GameRa.Modules.Distribution.Application.Abstractions.Storage;

/// <summary>
/// A presigned upload URL plus the headers the uploader must send with the PUT request.
/// </summary>
public sealed record PresignedUpload(string Url, IReadOnlyDictionary<string, string> Headers);
