using FluentAssertions;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Distribution.Application.Games.AddDistributionGame;
using GameRa.Modules.Distribution.Application.Ownerships.ApplyGameOwnership;
using GameRa.Modules.Distribution.Domain.Games;
using GameRa.Modules.Distribution.IntegrationTests.Abstractions;
using GameRa.Modules.Library.IntegrationEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GameRa.Modules.Distribution.IntegrationTests.Ownerships;

public class OwnershipTests : BaseIntegrationTest
{
    private readonly IEventBus _eventBus;

    public OwnershipTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
        _eventBus = factory.Services.GetRequiredService<IEventBus>();
    }

    private async Task<bool> WaitForOwnershipAsync(Guid userId, Guid gameId, bool expected)
    {
        DateTime deadline = DateTime.UtcNow.AddSeconds(30);
        while (DateTime.UtcNow < deadline)
        {
            if (await OwnershipReader.OwnsAsync(userId, gameId) == expected)
            {
                return true;
            }

            await Task.Delay(TimeSpan.FromMilliseconds(500));
        }

        return false;
    }

    [Fact]
    public async Task Apply_ShouldGrantAndRevokeOwnership()
    {
        var userId = Guid.NewGuid();
        var gameId = Guid.NewGuid();
        DateTime now = DateTime.UtcNow;

        await Sender.Send(new ApplyGameOwnershipCommand(userId, gameId, true, now));
        (await OwnershipReader.OwnsAsync(userId, gameId)).Should().BeTrue();

        await Sender.Send(new ApplyGameOwnershipCommand(userId, gameId, false, now.AddSeconds(1)));
        (await OwnershipReader.OwnsAsync(userId, gameId)).Should().BeFalse();
    }

    [Fact]
    public async Task Apply_ShouldBeIdempotent_ForDuplicatedEvents()
    {
        var userId = Guid.NewGuid();
        var gameId = Guid.NewGuid();
        DateTime now = DateTime.UtcNow;

        Result first = await Sender.Send(new ApplyGameOwnershipCommand(userId, gameId, true, now));
        Result second = await Sender.Send(new ApplyGameOwnershipCommand(userId, gameId, true, now));

        first.IsSuccess.Should().BeTrue();
        second.IsSuccess.Should().BeTrue();
        (await OwnershipReader.OwnsAsync(userId, gameId)).Should().BeTrue();
    }

    [Fact]
    public async Task Apply_ShouldIgnoreOutOfOrderEvents()
    {
        var userId = Guid.NewGuid();
        var gameId = Guid.NewGuid();
        DateTime addedAt = DateTime.UtcNow;

        // Removed arrives before the older Added event.
        await Sender.Send(new ApplyGameOwnershipCommand(userId, gameId, false, addedAt.AddSeconds(5)));
        await Sender.Send(new ApplyGameOwnershipCommand(userId, gameId, true, addedAt));

        (await OwnershipReader.OwnsAsync(userId, gameId)).Should().BeFalse();
    }

    [Fact]
    public async Task LibraryEvents_ShouldGrantAndRevokeOwnership_ThroughTheInbox()
    {
        var userId = Guid.NewGuid();
        var gameId = Guid.NewGuid();
        DateTime now = DateTime.UtcNow;

        await _eventBus.PublishAsync(new LibraryItemAddedIntegrationEvent(Guid.NewGuid(), now, userId, gameId));
        (await WaitForOwnershipAsync(userId, gameId, true)).Should().BeTrue();

        await _eventBus.PublishAsync(new LibraryItemRemovedIntegrationEvent(Guid.NewGuid(), now.AddSeconds(1), userId, gameId));
        (await WaitForOwnershipAsync(userId, gameId, false)).Should().BeTrue();
    }

    [Fact]
    public async Task AddDistributionGame_ShouldBeIdempotent()
    {
        var gameId = Guid.NewGuid();

        Result first = await Sender.Send(new AddDistributionGameCommand(gameId));
        Result second = await Sender.Send(new AddDistributionGameCommand(gameId));

        first.IsSuccess.Should().BeTrue();
        second.IsSuccess.Should().BeTrue();
        (await DbContext.Set<DistributionGame>().CountAsync(g => g.GameId == gameId)).Should().Be(1);
    }
}
