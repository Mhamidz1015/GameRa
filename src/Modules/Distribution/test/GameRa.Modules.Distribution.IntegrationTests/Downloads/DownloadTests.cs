using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Distribution.Application.Downloads;
using GameRa.Modules.Distribution.Application.Downloads.CheckForUpdate;
using GameRa.Modules.Distribution.Application.Downloads.GetFileDownloadUrl;
using GameRa.Modules.Distribution.Application.Downloads.GetFileDownloadUrls;
using GameRa.Modules.Distribution.Application.Downloads.GetLatestVersion;
using GameRa.Modules.Distribution.Application.Downloads.GetVersionManifest;
using GameRa.Modules.Distribution.Domain.Games;
using GameRa.Modules.Distribution.Domain.GameVersions;
using GameRa.Modules.Distribution.IntegrationTests.Abstractions;

namespace GameRa.Modules.Distribution.IntegrationTests.Downloads;

public class DownloadTests : BaseIntegrationTest
{
    public DownloadTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    private async Task<(Guid UserId, Guid GameId)> OwnedGameAsync()
    {
        var userId = Guid.NewGuid();
        Guid gameId = await Sender.AddGameAsync();
        await Sender.GrantOwnershipAsync(userId, gameId);
        return (userId, gameId);
    }

    [Fact]
    public async Task GetLatest_ShouldIgnoreDraftAndReadyVersions()
    {
        (Guid userId, Guid gameId) = await OwnedGameAsync();
        TestVersion published = await Sender.CreateVersionInStatusAsync(FileStorage, gameId, "1.0.0", GameVersionStatus.Published);
        await Sender.CreateVersionInStatusAsync(FileStorage, gameId, "2.0.0", GameVersionStatus.Ready);
        await Sender.CreateVersionInStatusAsync(FileStorage, gameId, "3.0.0", GameVersionStatus.Draft);

        Result<LatestVersionResponse> result = await Sender.Send(new GetLatestVersionQuery(userId, gameId));

        result.Value.VersionId.Should().Be(published.VersionId);
        result.Value.Version.Should().Be("1.0.0");
        result.Value.FileCount.Should().Be(2);
        result.Value.TotalSizeBytes.Should().Be(350);
    }

    [Fact]
    public async Task GetLatest_ShouldCompareVersionsNumerically()
    {
        (Guid userId, Guid gameId) = await OwnedGameAsync();
        TestVersion newest = await Sender.CreateVersionInStatusAsync(FileStorage, gameId, "1.10.0", GameVersionStatus.Published);
        await Sender.CreateVersionInStatusAsync(FileStorage, gameId, "1.9.0", GameVersionStatus.Published);

        Result<LatestVersionResponse> result = await Sender.Send(new GetLatestVersionQuery(userId, gameId));

        result.Value.VersionId.Should().Be(newest.VersionId);
    }

    [Fact]
    public async Task GetLatest_ShouldSkipDeprecatedVersion()
    {
        (Guid userId, Guid gameId) = await OwnedGameAsync();
        TestVersion stable = await Sender.CreateVersionInStatusAsync(FileStorage, gameId, "1.0.0", GameVersionStatus.Published);
        (await Sender.Send(new GetLatestVersionQuery(userId, gameId))).Value.VersionId.Should().Be(stable.VersionId);
        await Sender.CreateVersionInStatusAsync(FileStorage, gameId, "1.1.0", GameVersionStatus.Deprecated);

        Result<LatestVersionResponse> result = await Sender.Send(new GetLatestVersionQuery(userId, gameId));

        result.Value.VersionId.Should().Be(stable.VersionId);
    }

    [Fact]
    public async Task GetLatest_ShouldReturnNotFound_WhenNothingIsPublished()
    {
        (Guid userId, Guid gameId) = await OwnedGameAsync();

        Result<LatestVersionResponse> result = await Sender.Send(new GetLatestVersionQuery(userId, gameId));

        result.Error.Should().Be(GameVersionErrors.NoPublishedVersion(gameId));
    }

    [Fact]
    public async Task GetLatest_ShouldReturnForbidden_WhenUserDoesNotOwnGame()
    {
        Guid gameId = await Sender.AddGameAsync();
        await Sender.CreateVersionInStatusAsync(FileStorage, gameId, "1.0.0", GameVersionStatus.Published);

        Result<LatestVersionResponse> result = await Sender.Send(new GetLatestVersionQuery(Guid.NewGuid(), gameId));

        result.Error.Should().Be(DistributionErrors.GameNotOwned(gameId));
        result.Error.Type.Should().Be(ErrorType.Forbidden);
    }

    [Theory]
    [InlineData(GameVersionStatus.Draft)]
    [InlineData(GameVersionStatus.Ready)]
    public async Task GetManifest_ShouldReturnNotFound_ForUnpublishedVersions(GameVersionStatus status)
    {
        (Guid userId, Guid gameId) = await OwnedGameAsync();
        TestVersion version = await Sender.CreateVersionInStatusAsync(FileStorage, gameId, "1.0.0", status);

        Result<VersionManifestResponse> result =
            await Sender.Send(new GetVersionManifestQuery(userId, gameId, version.VersionId));

        result.Error.Should().Be(GameVersionErrors.NotFound(version.VersionId));
    }

    [Theory]
    [InlineData(GameVersionStatus.Published)]
    [InlineData(GameVersionStatus.Deprecated)]
    public async Task GetManifest_ShouldReturnFilesAndTotals_ForPublishedAndDeprecatedVersions(GameVersionStatus status)
    {
        (Guid userId, Guid gameId) = await OwnedGameAsync();
        TestVersion version = await Sender.CreateVersionInStatusAsync(FileStorage, gameId, "1.0.0", status, 100, 200, 300);

        Result<VersionManifestResponse> result =
            await Sender.Send(new GetVersionManifestQuery(userId, gameId, version.VersionId));

        result.IsSuccess.Should().BeTrue();
        result.Value.Status.Should().Be(status.ToString());
        result.Value.FileCount.Should().Be(3);
        result.Value.TotalSizeBytes.Should().Be(600);
        result.Value.Files.Select(f => f.Sha256).Should().BeEquivalentTo(version.Files.Select(f => f.Sha256));
        result.Value.Files.Select(f => f.RelativePath).Should().BeEquivalentTo(version.Files.Select(f => f.RelativePath));
    }

    [Fact]
    public async Task GetManifest_ShouldReturnForbidden_WhenUserDoesNotOwnGame()
    {
        Guid gameId = await Sender.AddGameAsync();
        TestVersion version = await Sender.CreateVersionInStatusAsync(FileStorage, gameId, "1.0.0", GameVersionStatus.Published);

        Result<VersionManifestResponse> result =
            await Sender.Send(new GetVersionManifestQuery(Guid.NewGuid(), gameId, version.VersionId));

        result.Error.Should().Be(DistributionErrors.GameNotOwned(gameId));
    }

    [Fact]
    public async Task GetDownloadUrl_ShouldReturnSignedUrl_ForOwner()
    {
        (Guid userId, Guid gameId) = await OwnedGameAsync();
        TestVersion version = await Sender.CreateVersionInStatusAsync(FileStorage, gameId, "1.0.0", GameVersionStatus.Published);
        TestFile file = version.Files[0];

        Result<FileDownloadUrlResponse> result = await Sender.Send(new GetFileDownloadUrlQuery(userId, file.FileId));

        result.IsSuccess.Should().BeTrue();
        result.Value.Url.Should().Contain(file.StorageKey);
        result.Value.Sha256.Should().Be(file.Sha256);
        result.Value.SizeBytes.Should().Be(file.SizeBytes);
        result.Value.ExpiresAtUtc.Should().BeAfter(DateTime.UtcNow);
    }

    [Fact]
    public async Task GetDownloadUrl_ShouldReturnForbidden_ForNonOwner()
    {
        Guid gameId = await Sender.AddGameAsync();
        TestVersion version = await Sender.CreateVersionInStatusAsync(FileStorage, gameId, "1.0.0", GameVersionStatus.Published);

        Result<FileDownloadUrlResponse> result =
            await Sender.Send(new GetFileDownloadUrlQuery(Guid.NewGuid(), version.Files[0].FileId));

        result.Error.Should().Be(DistributionErrors.GameNotOwned(gameId));
    }

    [Fact]
    public async Task GetDownloadUrl_ShouldReturnNotFound_ForDraftFile()
    {
        (Guid userId, Guid gameId) = await OwnedGameAsync();
        TestVersion version = await Sender.CreateVersionInStatusAsync(FileStorage, gameId, "1.0.0", GameVersionStatus.Draft);
        Guid fileId = version.Files[0].FileId;

        Result<FileDownloadUrlResponse> result = await Sender.Send(new GetFileDownloadUrlQuery(userId, fileId));

        result.Error.Should().Be(GameVersionErrors.NotDownloadable(fileId));
        result.Error.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task GetDownloadUrls_ShouldReturnUrlsForAllFiles()
    {
        (Guid userId, Guid gameId) = await OwnedGameAsync();
        TestVersion version = await Sender.CreateVersionInStatusAsync(FileStorage, gameId, "1.0.0", GameVersionStatus.Deprecated, 1, 2, 3);

        Result<IReadOnlyCollection<FileDownloadUrlResponse>> result = await Sender.Send(
            new GetFileDownloadUrlsQuery(userId, version.Files.Select(f => f.FileId).ToList()));

        result.Value.Select(r => r.FileId).Should().BeEquivalentTo(version.Files.Select(f => f.FileId));
    }

    [Fact]
    public async Task GetDownloadUrls_ShouldFailValidation_WhenBatchIsTooLarge()
    {
        var fileIds = Enumerable.Range(0, 201).Select(_ => Guid.NewGuid()).ToList();

        Result<IReadOnlyCollection<FileDownloadUrlResponse>> result =
            await Sender.Send(new GetFileDownloadUrlsQuery(Guid.NewGuid(), fileIds));

        result.Error.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task GetDownloadUrls_ShouldForbidWholeBatch_WhenAnyFileBelongsToNotOwnedGame()
    {
        (Guid userId, Guid ownedGameId) = await OwnedGameAsync();
        TestVersion owned = await Sender.CreateVersionInStatusAsync(FileStorage, ownedGameId, "1.0.0", GameVersionStatus.Published);
        Guid otherGameId = await Sender.AddGameAsync();
        TestVersion other = await Sender.CreateVersionInStatusAsync(FileStorage, otherGameId, "1.0.0", GameVersionStatus.Published);

        Result<IReadOnlyCollection<FileDownloadUrlResponse>> result = await Sender.Send(
            new GetFileDownloadUrlsQuery(userId, [owned.Files[0].FileId, other.Files[0].FileId]));

        result.Error.Should().Be(DistributionErrors.GameNotOwned(otherGameId));
    }

    [Fact]
    public async Task CheckForUpdate_ShouldReportUpdate_WhenClientIsBehind()
    {
        (Guid userId, Guid gameId) = await OwnedGameAsync();
        TestVersion latest = await Sender.CreateVersionInStatusAsync(FileStorage, gameId, "1.10.0", GameVersionStatus.Published);

        Result<CheckForUpdateResponse> result = await Sender.Send(new CheckForUpdateQuery(userId, gameId, "1.9.5"));

        result.Value.Should().Be(new CheckForUpdateResponse(true, "1.9.5", "1.10.0", latest.VersionId));
    }

    [Fact]
    public async Task CheckForUpdate_ShouldReportNoUpdate_WhenClientIsOnLatest()
    {
        (Guid userId, Guid gameId) = await OwnedGameAsync();
        TestVersion latest = await Sender.CreateVersionInStatusAsync(FileStorage, gameId, "2.0.0", GameVersionStatus.Published);

        Result<CheckForUpdateResponse> result = await Sender.Send(new CheckForUpdateQuery(userId, gameId, "2.0.0"));

        result.Value.Should().Be(new CheckForUpdateResponse(false, "2.0.0", "2.0.0", latest.VersionId));
    }

    [Fact]
    public async Task CheckForUpdate_ShouldFailValidation_WhenVersionIsInvalid()
    {
        (Guid userId, Guid gameId) = await OwnedGameAsync();

        Result<CheckForUpdateResponse> result = await Sender.Send(new CheckForUpdateQuery(userId, gameId, "1.x"));

        result.Error.Type.Should().Be(ErrorType.Validation);
    }
}
