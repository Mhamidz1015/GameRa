using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Library.Application.LibraryItems.AddGameToLibrary;
using GameRa.Modules.Library.Application.Playtime.GetPlaytimeSummary;
using GameRa.Modules.Library.Application.Playtime.RecordPlaytime;
using GameRa.Modules.Library.Domain.LibraryItems;
using GameRa.Modules.Library.IntegrationTests.Abstractions;

namespace GameRa.Modules.Library.IntegrationTests.Playtime;

public sealed class PlaytimeTests : BaseIntegrationTest
{
    public PlaytimeTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    [Fact]
    public async Task RecordPlaytime_ShouldSucceed_WhenUserOwnsGame()
    {
        Guid userId = Faker.Random.Guid();
        Guid gameId = Faker.Random.Guid();

        await Sender.AddGameToLibraryAsync(userId, gameId);

        Result result = await Sender.Send(new RecordPlaytimeCommand(userId, gameId, 30));

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task RecordPlaytime_ShouldFail_WhenUserDoesNotOwnGame()
    {
        Result result = await Sender.Send(
            new RecordPlaytimeCommand(Faker.Random.Guid(), Faker.Random.Guid(), 30));

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(LibraryItemErrors.NotOwned);
    }

    [Fact]
    public async Task RecordPlaytime_ShouldAccumulate_OnMultipleSessions()
    {
        Guid userId = Faker.Random.Guid();
        Guid gameId = Faker.Random.Guid();

        await Sender.AddGameToLibraryAsync(userId, gameId);

        await Sender.Send(new RecordPlaytimeCommand(userId, gameId, 30));
        await Sender.Send(new RecordPlaytimeCommand(userId, gameId, 45));
        await Sender.Send(new RecordPlaytimeCommand(userId, gameId, 15));

        Result<IReadOnlyCollection<PlaytimeSummaryResponse>> result =
            await Sender.Send(new GetPlaytimeSummaryQuery(userId));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().ContainSingle(p => p.GameId == gameId);
        result.Value.First(p => p.GameId == gameId).TotalMinutes.Should().Be(90);
    }

    [Fact]
    public async Task GetPlaytimeSummary_ShouldReturnAllGames()
    {
        Guid userId = Faker.Random.Guid();
        Guid gameId1 = Faker.Random.Guid();
        Guid gameId2 = Faker.Random.Guid();

        await Sender.AddGameToLibraryAsync(userId, gameId1);
        await Sender.AddGameToLibraryAsync(userId, gameId2);

        await Sender.Send(new RecordPlaytimeCommand(userId, gameId1, 60));
        await Sender.Send(new RecordPlaytimeCommand(userId, gameId2, 120));

        Result<IReadOnlyCollection<PlaytimeSummaryResponse>> result =
            await Sender.Send(new GetPlaytimeSummaryQuery(userId));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetPlaytimeSummary_ShouldReturnEmpty_WhenNoPlaytime()
    {
        Result<IReadOnlyCollection<PlaytimeSummaryResponse>> result =
            await Sender.Send(new GetPlaytimeSummaryQuery(Faker.Random.Guid()));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }
}