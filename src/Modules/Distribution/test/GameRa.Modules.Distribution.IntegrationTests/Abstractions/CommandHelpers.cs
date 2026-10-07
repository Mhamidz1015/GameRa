using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Distribution.Application.Games.AddDistributionGame;
using GameRa.Modules.Distribution.Application.GameVersions.ConfirmVersionFileUpload;
using GameRa.Modules.Distribution.Application.GameVersions.CreateGameVersion;
using GameRa.Modules.Distribution.Application.GameVersions.DeprecateGameVersion;
using GameRa.Modules.Distribution.Application.GameVersions.MarkVersionReady;
using GameRa.Modules.Distribution.Application.GameVersions.PublishGameVersion;
using GameRa.Modules.Distribution.Application.GameVersions.RegisterVersionFile;
using GameRa.Modules.Distribution.Application.Ownerships.ApplyGameOwnership;
using GameRa.Modules.Distribution.Domain.GameVersions;
using MediatR;

namespace GameRa.Modules.Distribution.IntegrationTests.Abstractions;

internal sealed record TestVersion(Guid GameId, Guid VersionId, IReadOnlyList<TestFile> Files);

internal sealed record TestFile(Guid FileId, string RelativePath, long SizeBytes, string Sha256, string StorageKey);

internal static class CommandHelpers
{
    internal static async Task<Guid> AddGameAsync(this ISender sender)
    {
        var gameId = Guid.NewGuid();

        Result result = await sender.Send(new AddDistributionGameCommand(gameId));

        result.IsSuccess.Should().BeTrue();

        return gameId;
    }

    internal static async Task GrantOwnershipAsync(this ISender sender, Guid userId, Guid gameId)
    {
        Result result = await sender.Send(new ApplyGameOwnershipCommand(userId, gameId, true, DateTime.UtcNow));

        result.IsSuccess.Should().BeTrue();
    }

    internal static async Task<Guid> CreateVersionAsync(this ISender sender, Guid gameId, string version)
    {
        Result<Guid> result = await sender.Send(new CreateGameVersionCommand(gameId, version, "Release notes"));

        result.IsSuccess.Should().BeTrue();

        return result.Value;
    }

    internal static async Task<TestFile> RegisterFileAsync(
        this ISender sender,
        Guid gameId,
        Guid versionId,
        string relativePath,
        long sizeBytes,
        string sha256)
    {
        Result<RegisterVersionFileResponse> result = await sender.Send(
            new RegisterVersionFileCommand(versionId, relativePath, sizeBytes, sha256));

        result.IsSuccess.Should().BeTrue();

        return new TestFile(
            result.Value.FileId,
            relativePath,
            sizeBytes,
            sha256,
            $"games/{gameId}/versions/{versionId}/{result.Value.FileId}");
    }

    internal static async Task<TestVersion> CreateVersionInStatusAsync(
        this ISender sender,
        FakeFileStorage fileStorage,
        Guid gameId,
        string version,
        GameVersionStatus status,
        params long[] fileSizes)
    {
        if (fileSizes.Length == 0)
        {
            fileSizes = [100, 250];
        }

        Guid versionId = await sender.CreateVersionAsync(gameId, version);

        var files = new List<TestFile>();
        for (int i = 0; i < fileSizes.Length; i++)
        {
            string sha256 = Convert.ToHexStringLower(Guid.NewGuid().ToByteArray().Concat(Guid.NewGuid().ToByteArray()).ToArray());
            TestFile file = await sender.RegisterFileAsync(gameId, versionId, $"Data/file{i}.pak", fileSizes[i], sha256);
            files.Add(file);
        }

        if (status == GameVersionStatus.Draft)
        {
            return new TestVersion(gameId, versionId, files);
        }

        foreach (TestFile file in files)
        {
            fileStorage.SimulateUpload(file.StorageKey, file.SizeBytes, file.Sha256);
            (await sender.Send(new ConfirmVersionFileUploadCommand(versionId, file.FileId))).IsSuccess.Should().BeTrue();
        }

        (await sender.Send(new MarkVersionReadyCommand(versionId))).IsSuccess.Should().BeTrue();

        if (status >= GameVersionStatus.Published)
        {
            (await sender.Send(new PublishGameVersionCommand(versionId))).IsSuccess.Should().BeTrue();
        }

        if (status == GameVersionStatus.Deprecated)
        {
            (await sender.Send(new DeprecateGameVersionCommand(versionId))).IsSuccess.Should().BeTrue();
        }

        return new TestVersion(gameId, versionId, files);
    }
}
