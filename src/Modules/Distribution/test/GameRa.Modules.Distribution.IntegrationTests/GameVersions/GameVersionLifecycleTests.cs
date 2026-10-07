using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Distribution.Application.GameVersions.ConfirmVersionFileUpload;
using GameRa.Modules.Distribution.Application.GameVersions.CreateGameVersion;
using GameRa.Modules.Distribution.Application.GameVersions.DeprecateGameVersion;
using GameRa.Modules.Distribution.Application.GameVersions.GetGameVersions;
using GameRa.Modules.Distribution.Application.GameVersions.MarkVersionReady;
using GameRa.Modules.Distribution.Application.GameVersions.PublishGameVersion;
using GameRa.Modules.Distribution.Application.GameVersions.RegisterVersionFile;
using GameRa.Modules.Distribution.Application.GameVersions.RemoveVersionFile;
using GameRa.Modules.Distribution.Domain.Games;
using GameRa.Modules.Distribution.Domain.GameVersions;
using GameRa.Modules.Distribution.IntegrationTests.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GameRa.Modules.Distribution.IntegrationTests.GameVersions;

public class GameVersionLifecycleTests : BaseIntegrationTest
{
    public GameVersionLifecycleTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    private async Task<GameVersion> LoadAsync(Guid versionId) =>
        await DbContext.Set<GameVersion>().AsNoTracking().Include(v => v.Files).SingleAsync(v => v.Id == versionId);

    [Fact]
    public async Task CreateGameVersion_ShouldFail_WhenGameIsUnknown()
    {
        var gameId = Guid.NewGuid();

        Result<Guid> result = await Sender.Send(new CreateGameVersionCommand(gameId, "1.0.0", null));

        result.Error.Should().Be(DistributionErrors.GameNotFound(gameId));
    }

    [Fact]
    public async Task CreateGameVersion_ShouldFail_WhenVersionAlreadyExists()
    {
        Guid gameId = await Sender.AddGameAsync();
        await Sender.CreateVersionAsync(gameId, "1.0.0");

        Result<Guid> result = await Sender.Send(new CreateGameVersionCommand(gameId, "1.0.0", null));

        result.Error.Should().Be(GameVersionErrors.VersionAlreadyExists(gameId, "1.0.0"));
        result.Error.Type.Should().Be(ErrorType.Conflict);
    }

    [Fact]
    public async Task CreateGameVersion_ShouldFail_WhenVersionIsInvalid()
    {
        Guid gameId = await Sender.AddGameAsync();

        Result<Guid> result = await Sender.Send(new CreateGameVersionCommand(gameId, "01.2", null));

        result.Error.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task CreateGameVersion_ShouldCreateDraft()
    {
        Guid gameId = await Sender.AddGameAsync();

        Result<Guid> result = await Sender.Send(new CreateGameVersionCommand(gameId, "1.2.3", "First build"));

        result.IsSuccess.Should().BeTrue();
        GameVersion version = await LoadAsync(result.Value);
        version.Status.Should().Be(GameVersionStatus.Draft);
        version.Number.ToString().Should().Be("1.2.3");
        version.ReleaseNotes.Should().Be("First build");
    }

    [Fact]
    public async Task RegisterVersionFile_ShouldReturnUploadUrl()
    {
        Guid gameId = await Sender.AddGameAsync();
        Guid versionId = await Sender.CreateVersionAsync(gameId, "1.0.0");

        Result<RegisterVersionFileResponse> result = await Sender.Send(
            new RegisterVersionFileCommand(versionId, @"Bin\Game.exe", 1024, Sha256()));

        result.IsSuccess.Should().BeTrue();
        result.Value.UploadUrl.Should().Contain($"games/{gameId}/versions/{versionId}/{result.Value.FileId}");
        result.Value.UploadHeaders.Should().ContainKey("x-amz-checksum-sha256");
        result.Value.ExpiresAtUtc.Should().BeAfter(DateTime.UtcNow);
        GameVersion version = await LoadAsync(versionId);
        version.Files.Single().RelativePath.Should().Be("Bin/Game.exe");
        version.Files.Single().UploadStatus.Should().Be(FileUploadStatus.Pending);
    }

    [Fact]
    public async Task RegisterVersionFile_ShouldFail_WhenFileIsTooLarge()
    {
        Guid gameId = await Sender.AddGameAsync();
        Guid versionId = await Sender.CreateVersionAsync(gameId, "1.0.0");

        Result<RegisterVersionFileResponse> result = await Sender.Send(
            new RegisterVersionFileCommand(versionId, "huge.pak", 5L * 1024 * 1024 * 1024 + 1, Sha256()));

        result.Error.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task RegisterVersionFile_ShouldFail_WhenVersionIsNotDraft()
    {
        Guid gameId = await Sender.AddGameAsync();
        TestVersion version = await Sender.CreateVersionInStatusAsync(FileStorage, gameId, "1.0.0", GameVersionStatus.Ready);

        Result<RegisterVersionFileResponse> result = await Sender.Send(
            new RegisterVersionFileCommand(version.VersionId, "late.pak", 10, Sha256()));

        result.Error.Should().Be(
            GameVersionErrors.InvalidStatusTransition(GameVersionStatus.Ready, GameVersionStatus.Ready));
    }

    [Fact]
    public async Task RegisterVersionFile_ShouldFail_AfterPublish()
    {
        Guid gameId = await Sender.AddGameAsync();
        TestVersion version = await Sender.CreateVersionInStatusAsync(FileStorage, gameId, "1.0.0", GameVersionStatus.Published);

        Result<RegisterVersionFileResponse> result = await Sender.Send(
            new RegisterVersionFileCommand(version.VersionId, "patch.pak", 10, Sha256()));

        result.Error.Should().Be(
            GameVersionErrors.InvalidStatusTransition(GameVersionStatus.Published, GameVersionStatus.Published));
        (await LoadAsync(version.VersionId)).FileCount.Should().Be(2);
    }

    [Fact]
    public async Task RemoveVersionFile_ShouldRemoveFileAndStoredObject()
    {
        Guid gameId = await Sender.AddGameAsync();
        Guid versionId = await Sender.CreateVersionAsync(gameId, "1.0.0");
        TestFile file = await Sender.RegisterFileAsync(gameId, versionId, "game.exe", 10, Sha256());
        FileStorage.SimulateUpload(file.StorageKey, file.SizeBytes, file.Sha256);

        Result result = await Sender.Send(new RemoveVersionFileCommand(versionId, file.FileId));

        result.IsSuccess.Should().BeTrue();
        (await LoadAsync(versionId)).Files.Should().BeEmpty();
        FileStorage.Contains(file.StorageKey).Should().BeFalse();
    }

    [Fact]
    public async Task ConfirmUpload_ShouldFail_WhenObjectWasNotUploaded()
    {
        Guid gameId = await Sender.AddGameAsync();
        Guid versionId = await Sender.CreateVersionAsync(gameId, "1.0.0");
        TestFile file = await Sender.RegisterFileAsync(gameId, versionId, "game.exe", 10, Sha256());

        Result result = await Sender.Send(new ConfirmVersionFileUploadCommand(versionId, file.FileId));

        result.Error.Should().Be(GameVersionErrors.UploadNotFound(file.FileId));
    }

    [Fact]
    public async Task ConfirmUpload_ShouldFail_WhenSizeDoesNotMatch()
    {
        Guid gameId = await Sender.AddGameAsync();
        Guid versionId = await Sender.CreateVersionAsync(gameId, "1.0.0");
        TestFile file = await Sender.RegisterFileAsync(gameId, versionId, "game.exe", 10, Sha256());
        FileStorage.SimulateUpload(file.StorageKey, 11, file.Sha256);

        Result result = await Sender.Send(new ConfirmVersionFileUploadCommand(versionId, file.FileId));

        result.Error.Should().Be(GameVersionErrors.UploadSizeMismatch(file.FileId));
    }

    [Fact]
    public async Task ConfirmUpload_ShouldFail_WhenHashDoesNotMatch_AndSucceedAfterOverwrite()
    {
        Guid gameId = await Sender.AddGameAsync();
        Guid versionId = await Sender.CreateVersionAsync(gameId, "1.0.0");
        TestFile file = await Sender.RegisterFileAsync(gameId, versionId, "game.exe", 10, Sha256());
        FileStorage.SimulateUpload(file.StorageKey, file.SizeBytes, Sha256());

        Result wrongHash = await Sender.Send(new ConfirmVersionFileUploadCommand(versionId, file.FileId));

        wrongHash.Error.Should().Be(GameVersionErrors.UploadChecksumMismatch(file.FileId));
        FileStorage.Contains(file.StorageKey).Should().BeTrue();

        FileStorage.SimulateUpload(file.StorageKey, file.SizeBytes, file.Sha256);
        Result fixedUpload = await Sender.Send(new ConfirmVersionFileUploadCommand(versionId, file.FileId));

        fixedUpload.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task ConfirmUpload_ShouldVerifyFile_AndBeIdempotent()
    {
        Guid gameId = await Sender.AddGameAsync();
        Guid versionId = await Sender.CreateVersionAsync(gameId, "1.0.0");
        TestFile file = await Sender.RegisterFileAsync(gameId, versionId, "game.exe", 10, Sha256());
        FileStorage.SimulateUpload(file.StorageKey, file.SizeBytes, file.Sha256);

        Result first = await Sender.Send(new ConfirmVersionFileUploadCommand(versionId, file.FileId));
        Result second = await Sender.Send(new ConfirmVersionFileUploadCommand(versionId, file.FileId));

        first.IsSuccess.Should().BeTrue();
        second.IsSuccess.Should().BeTrue();
        (await LoadAsync(versionId)).Files.Single().UploadStatus.Should().Be(FileUploadStatus.Verified);
    }

    [Fact]
    public async Task MarkReady_ShouldFail_WhenFilesAreNotVerified()
    {
        Guid gameId = await Sender.AddGameAsync();
        TestVersion version = await Sender.CreateVersionInStatusAsync(FileStorage, gameId, "1.0.0", GameVersionStatus.Draft);

        Result result = await Sender.Send(new MarkVersionReadyCommand(version.VersionId));

        result.Error.Should().Be(GameVersionErrors.UnverifiedFiles);
    }

    [Fact]
    public async Task ReadyPublishDeprecate_ShouldUpdateStatus_AndWriteOutboxMessages()
    {
        Guid gameId = await Sender.AddGameAsync();
        TestVersion version = await Sender.CreateVersionInStatusAsync(FileStorage, gameId, "1.0.0", GameVersionStatus.Ready);
        (await LoadAsync(version.VersionId)).Status.Should().Be(GameVersionStatus.Ready);

        Result publish = await Sender.Send(new PublishGameVersionCommand(version.VersionId));
        Result publishAgain = await Sender.Send(new PublishGameVersionCommand(version.VersionId));

        publish.IsSuccess.Should().BeTrue();
        publishAgain.IsSuccess.Should().BeTrue();
        GameVersion published = await LoadAsync(version.VersionId);
        published.Status.Should().Be(GameVersionStatus.Published);
        published.PublishedOnUtc.Should().NotBeNull();
        (await CountOutboxMessagesAsync(nameof(GameVersionPublishedDomainEvent), version.VersionId)).Should().Be(1);

        Result deprecate = await Sender.Send(new DeprecateGameVersionCommand(version.VersionId));

        deprecate.IsSuccess.Should().BeTrue();
        GameVersion deprecated = await LoadAsync(version.VersionId);
        deprecated.Status.Should().Be(GameVersionStatus.Deprecated);
        deprecated.DeprecatedOnUtc.Should().NotBeNull();
        (await CountOutboxMessagesAsync(nameof(GameVersionDeprecatedDomainEvent), version.VersionId)).Should().Be(1);
    }

    [Fact]
    public async Task Publish_ShouldFail_WhenVersionIsDraft()
    {
        Guid gameId = await Sender.AddGameAsync();
        Guid versionId = await Sender.CreateVersionAsync(gameId, "1.0.0");

        Result result = await Sender.Send(new PublishGameVersionCommand(versionId));

        result.Error.Should().Be(
            GameVersionErrors.InvalidStatusTransition(GameVersionStatus.Draft, GameVersionStatus.Published));
    }

    [Fact]
    public async Task Publish_ShouldFail_WhenVersionDoesNotExist()
    {
        var versionId = Guid.NewGuid();

        Result result = await Sender.Send(new PublishGameVersionCommand(versionId));

        result.Error.Should().Be(GameVersionErrors.NotFound(versionId));
    }

    [Fact]
    public async Task GetGameVersions_ShouldReturnAllVersionsOrderedBySemanticVersion()
    {
        Guid gameId = await Sender.AddGameAsync();
        await Sender.CreateVersionInStatusAsync(FileStorage, gameId, "1.9.0", GameVersionStatus.Published, 10, 20);
        await Sender.CreateVersionInStatusAsync(FileStorage, gameId, "1.10.0", GameVersionStatus.Draft, 5);

        Result<IReadOnlyCollection<GameVersionResponse>> result = await Sender.Send(new GetGameVersionsQuery(gameId));

        result.Value.Select(v => v.Version).Should().Equal("1.10.0", "1.9.0");
        GameVersionResponse published = result.Value.Last();
        published.Status.Should().Be("Published");
        published.FileCount.Should().Be(2);
        published.VerifiedFileCount.Should().Be(2);
        published.TotalSizeBytes.Should().Be(30);
    }
}
