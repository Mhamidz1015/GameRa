using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Distribution.Domain.GameVersions;
using GameRa.Modules.Distribution.UnitTests.Abstractions;

namespace GameRa.Modules.Distribution.UnitTests.GameVersions;

public class GameVersionTests : BaseTest
{
    private static GameVersion CreateDraft(string version = "1.0.0") =>
        GameVersion.CreateDraft(Guid.NewGuid(), Version(version), "notes", DateTime.UtcNow).Value;

    private static GameVersion CreateWithVerifiedFile()
    {
        GameVersion gameVersion = CreateDraft();
        VersionFile file = gameVersion.AddFile("game.exe", 100, Sha256()).Value;
        gameVersion.MarkFileVerified(file.Id);
        return gameVersion;
    }

    private static GameVersion CreateReady()
    {
        GameVersion gameVersion = CreateWithVerifiedFile();
        gameVersion.MarkReady().IsSuccess.Should().BeTrue();
        return gameVersion;
    }

    private static GameVersion CreatePublished()
    {
        GameVersion gameVersion = CreateReady();
        gameVersion.Publish(DateTime.UtcNow).IsSuccess.Should().BeTrue();
        gameVersion.ClearDomainEvents();
        return gameVersion;
    }

    private static GameVersion CreateDeprecated()
    {
        GameVersion gameVersion = CreatePublished();
        gameVersion.Deprecate(DateTime.UtcNow).IsSuccess.Should().BeTrue();
        gameVersion.ClearDomainEvents();
        return gameVersion;
    }

    [Fact]
    public void CreateDraft_ShouldCreateDraftVersion()
    {
        var gameId = Guid.NewGuid();
        DateTime utcNow = DateTime.UtcNow;

        Result<GameVersion> result = GameVersion.CreateDraft(gameId, Version("1.2.3"), "notes", utcNow);

        result.IsSuccess.Should().BeTrue();
        result.Value.GameId.Should().Be(gameId);
        result.Value.Number.Should().Be(Version("1.2.3"));
        result.Value.Status.Should().Be(GameVersionStatus.Draft);
        result.Value.CreatedOnUtc.Should().Be(utcNow);
        result.Value.PublishedOnUtc.Should().BeNull();
        result.Value.Files.Should().BeEmpty();
    }

    [Fact]
    public void CreateDraft_ShouldFail_WhenReleaseNotesAreTooLong()
    {
        Result<GameVersion> result = GameVersion.CreateDraft(
            Guid.NewGuid(), Version("1.0.0"), new string('a', GameVersion.MaxReleaseNotesLength + 1), DateTime.UtcNow);

        result.Error.Should().Be(GameVersionErrors.ReleaseNotesTooLong);
    }

    [Fact]
    public void AddFile_ShouldBuildStorageKeyAndNormalizePath()
    {
        GameVersion gameVersion = CreateDraft();
        string sha256 = Sha256().ToUpperInvariant();

        Result<VersionFile> result = gameVersion.AddFile(@"Data\Maps\Level1.PAK", 42, sha256);

        result.IsSuccess.Should().BeTrue();
        result.Value.RelativePath.Should().Be("Data/Maps/Level1.PAK");
        result.Value.NormalizedPath.Should().Be("data/maps/level1.pak");
        result.Value.Sha256.Should().Be(sha256.ToLowerInvariant());
        result.Value.UploadStatus.Should().Be(FileUploadStatus.Pending);
        result.Value.StorageKey.Should().Be($"games/{gameVersion.GameId}/versions/{gameVersion.Id}/{result.Value.Id}");
        result.Value.StorageKey.Should().NotContain("Level1");
    }

    [Fact]
    public void AddFile_ShouldFail_WhenPathDiffersOnlyByCase()
    {
        GameVersion gameVersion = CreateDraft();
        gameVersion.AddFile("Game.exe", 10, Sha256());

        Result<VersionFile> result = gameVersion.AddFile("game.EXE", 11, Sha256());

        result.Error.Should().Be(GameVersionErrors.FileAlreadyExists("game.EXE"));
    }

    [Fact]
    public void AddFile_ShouldReturnExistingFile_WhenSamePendingFileIsRegisteredAgain()
    {
        GameVersion gameVersion = CreateDraft();
        string sha256 = Sha256();
        VersionFile first = gameVersion.AddFile("game.exe", 10, sha256).Value;

        Result<VersionFile> result = gameVersion.AddFile("game.exe", 10, sha256);

        result.Value.Id.Should().Be(first.Id);
        gameVersion.FileCount.Should().Be(1);
    }

    [Theory]
    [InlineData("../game.exe")]
    [InlineData("data/../../game.exe")]
    [InlineData("/etc/passwd")]
    [InlineData(@"\game.exe")]
    [InlineData("C:/game.exe")]
    [InlineData("data//game.exe")]
    [InlineData("./game.exe")]
    [InlineData("")]
    [InlineData("   ")]
    public void AddFile_ShouldFail_WhenPathIsInvalid(string path)
    {
        GameVersion gameVersion = CreateDraft();

        Result<VersionFile> result = gameVersion.AddFile(path, 10, Sha256());

        result.Error.Should().Be(GameVersionErrors.InvalidPath(path));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void AddFile_ShouldFail_WhenSizeIsNotPositive(long size)
    {
        GameVersion gameVersion = CreateDraft();

        Result<VersionFile> result = gameVersion.AddFile("game.exe", size, Sha256());

        result.Error.Should().Be(GameVersionErrors.InvalidFile);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("zz00000000000000000000000000000000000000000000000000000000000000")]
    [InlineData("00000000000000000000000000000000000000000000000000000000000000000")]
    public void AddFile_ShouldFail_WhenSha256IsInvalid(string sha256)
    {
        GameVersion gameVersion = CreateDraft();

        Result<VersionFile> result = gameVersion.AddFile("game.exe", 10, sha256);

        result.Error.Should().Be(GameVersionErrors.InvalidFile);
    }

    [Fact]
    public void AddFile_ShouldFail_WhenVersionIsNotDraft()
    {
        GameVersion gameVersion = CreateReady();

        Result<VersionFile> result = gameVersion.AddFile("other.exe", 10, Sha256());

        result.Error.Should().Be(
            GameVersionErrors.InvalidStatusTransition(GameVersionStatus.Ready, GameVersionStatus.Ready));
    }

    [Fact]
    public void AddFile_ShouldFail_AfterPublish()
    {
        GameVersion gameVersion = CreatePublished();

        gameVersion.AddFile("other.exe", 10, Sha256()).IsFailure.Should().BeTrue();
    }

    [Fact]
    public void RemoveFile_ShouldRemoveFile_WhenDraft()
    {
        GameVersion gameVersion = CreateDraft();
        VersionFile file = gameVersion.AddFile("game.exe", 10, Sha256()).Value;

        Result result = gameVersion.RemoveFile(file.Id);

        result.IsSuccess.Should().BeTrue();
        gameVersion.Files.Should().BeEmpty();
    }

    [Fact]
    public void RemoveFile_ShouldFail_WhenFileDoesNotExist()
    {
        GameVersion gameVersion = CreateDraft();
        var fileId = Guid.NewGuid();

        gameVersion.RemoveFile(fileId).Error.Should().Be(GameVersionErrors.FileNotFound(fileId));
    }

    [Fact]
    public void RemoveFile_ShouldFail_WhenNotDraft()
    {
        GameVersion gameVersion = CreatePublished();

        Result result = gameVersion.RemoveFile(gameVersion.Files.Single().Id);

        result.Error.Should().Be(
            GameVersionErrors.InvalidStatusTransition(GameVersionStatus.Published, GameVersionStatus.Published));
        gameVersion.FileCount.Should().Be(1);
    }

    [Fact]
    public void MarkFileVerified_ShouldBeIdempotent()
    {
        GameVersion gameVersion = CreateDraft();
        VersionFile file = gameVersion.AddFile("game.exe", 10, Sha256()).Value;

        gameVersion.MarkFileVerified(file.Id).IsSuccess.Should().BeTrue();
        gameVersion.MarkFileVerified(file.Id).IsSuccess.Should().BeTrue();

        gameVersion.Files.Single().UploadStatus.Should().Be(FileUploadStatus.Verified);
    }

    [Fact]
    public void MarkReady_ShouldFail_WhenThereAreNoFiles()
    {
        CreateDraft().MarkReady().Error.Should().Be(GameVersionErrors.NoFiles);
    }

    [Fact]
    public void MarkReady_ShouldFail_WhenAFileIsNotVerified()
    {
        GameVersion gameVersion = CreateWithVerifiedFile();
        gameVersion.AddFile("data.pak", 10, Sha256());

        gameVersion.MarkReady().Error.Should().Be(GameVersionErrors.UnverifiedFiles);
    }

    [Fact]
    public void MarkReady_ShouldSucceed_WhenAllFilesAreVerified()
    {
        GameVersion gameVersion = CreateWithVerifiedFile();

        gameVersion.MarkReady().IsSuccess.Should().BeTrue();
        gameVersion.Status.Should().Be(GameVersionStatus.Ready);
    }

    [Fact]
    public void Publish_ShouldPublishAndRaiseEvent_WhenReady()
    {
        GameVersion gameVersion = CreateReady();
        DateTime utcNow = DateTime.UtcNow;

        Result result = gameVersion.Publish(utcNow);

        result.IsSuccess.Should().BeTrue();
        gameVersion.Status.Should().Be(GameVersionStatus.Published);
        gameVersion.PublishedOnUtc.Should().Be(utcNow);
        GameVersionPublishedDomainEvent domainEvent =
            AssertDomainEventWasPublished<GameVersionPublishedDomainEvent>(gameVersion);
        domainEvent.VersionId.Should().Be(gameVersion.Id);
        domainEvent.GameId.Should().Be(gameVersion.GameId);
        domainEvent.Version.Should().Be("1.0.0");
    }

    [Fact]
    public void Publish_ShouldNotRaiseSecondEvent_WhenAlreadyPublished()
    {
        GameVersion gameVersion = CreateReady();
        gameVersion.Publish(DateTime.UtcNow);

        Result result = gameVersion.Publish(DateTime.UtcNow.AddMinutes(1));

        result.IsSuccess.Should().BeTrue();
        gameVersion.DomainEvents.OfType<GameVersionPublishedDomainEvent>().Should().ContainSingle();
    }

    [Fact]
    public void Publish_ShouldFail_WhenDraft()
    {
        CreateWithVerifiedFile().Publish(DateTime.UtcNow).Error.Should().Be(
            GameVersionErrors.InvalidStatusTransition(GameVersionStatus.Draft, GameVersionStatus.Published));
    }

    [Fact]
    public void Publish_ShouldFail_WhenDeprecated()
    {
        CreateDeprecated().Publish(DateTime.UtcNow).Error.Should().Be(
            GameVersionErrors.InvalidStatusTransition(GameVersionStatus.Deprecated, GameVersionStatus.Published));
    }

    [Fact]
    public void Deprecate_ShouldDeprecateAndRaiseEvent_WhenPublished()
    {
        GameVersion gameVersion = CreatePublished();
        DateTime utcNow = DateTime.UtcNow;

        Result result = gameVersion.Deprecate(utcNow);

        result.IsSuccess.Should().BeTrue();
        gameVersion.Status.Should().Be(GameVersionStatus.Deprecated);
        gameVersion.DeprecatedOnUtc.Should().Be(utcNow);
        AssertDomainEventWasPublished<GameVersionDeprecatedDomainEvent>(gameVersion);
    }

    [Fact]
    public void Deprecate_ShouldBeIdempotent()
    {
        GameVersion gameVersion = CreateDeprecated();

        gameVersion.Deprecate(DateTime.UtcNow).IsSuccess.Should().BeTrue();
        gameVersion.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void Deprecate_ShouldFail_WhenDraft()
    {
        CreateDraft().Deprecate(DateTime.UtcNow).Error.Should().Be(
            GameVersionErrors.InvalidStatusTransition(GameVersionStatus.Draft, GameVersionStatus.Deprecated));
    }

    [Fact]
    public void Deprecate_ShouldFail_WhenReady()
    {
        CreateReady().Deprecate(DateTime.UtcNow).Error.Should().Be(
            GameVersionErrors.InvalidStatusTransition(GameVersionStatus.Ready, GameVersionStatus.Deprecated));
    }

    [Fact]
    public void MarkReady_ShouldFail_WhenPublished()
    {
        CreatePublished().MarkReady().Error.Should().Be(
            GameVersionErrors.InvalidStatusTransition(GameVersionStatus.Published, GameVersionStatus.Ready));
    }

    [Fact]
    public void MarkReady_ShouldFail_WhenDeprecated()
    {
        CreateDeprecated().MarkReady().Error.Should().Be(
            GameVersionErrors.InvalidStatusTransition(GameVersionStatus.Deprecated, GameVersionStatus.Ready));
    }

    [Fact]
    public void Totals_ShouldSumSizesAndCountFiles()
    {
        GameVersion gameVersion = CreateDraft();
        gameVersion.AddFile("a.bin", 100, Sha256());
        gameVersion.AddFile("b.bin", 250, Sha256());

        gameVersion.FileCount.Should().Be(2);
        gameVersion.TotalSizeBytes.Should().Be(350);
    }
}
