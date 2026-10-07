using FluentAssertions;
using GameRa.Modules.Distribution.Domain.Ownerships;
using GameRa.Modules.Distribution.UnitTests.Abstractions;

namespace GameRa.Modules.Distribution.UnitTests.Ownerships;

public class GameOwnershipTests : BaseTest
{
    [Fact]
    public void Apply_ShouldChangeOwnership_WhenEventIsNewer()
    {
        DateTime added = DateTime.UtcNow;
        var ownership = GameOwnership.Create(Guid.NewGuid(), Guid.NewGuid(), true, added);

        bool applied = ownership.Apply(false, added.AddSeconds(1));

        applied.Should().BeTrue();
        ownership.IsOwned.Should().BeFalse();
    }

    [Fact]
    public void Apply_ShouldIgnoreEvent_WhenEventIsOlder()
    {
        DateTime removed = DateTime.UtcNow;
        var ownership = GameOwnership.Create(Guid.NewGuid(), Guid.NewGuid(), false, removed);

        bool applied = ownership.Apply(true, removed.AddSeconds(-1));

        applied.Should().BeFalse();
        ownership.IsOwned.Should().BeFalse();
    }

    [Fact]
    public void Apply_ShouldIgnoreEvent_WhenEventIsDuplicated()
    {
        DateTime added = DateTime.UtcNow;
        var ownership = GameOwnership.Create(Guid.NewGuid(), Guid.NewGuid(), true, added);

        bool applied = ownership.Apply(true, added);

        applied.Should().BeFalse();
        ownership.IsOwned.Should().BeTrue();
    }
}
