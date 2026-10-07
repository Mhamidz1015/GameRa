namespace GameRa.Modules.Distribution.Application.Abstractions.Storage;

public sealed record StoredObjectInfo(bool Exists, long SizeBytes, string? Sha256)
{
    public static readonly StoredObjectInfo Missing = new(false, 0, null);
}
