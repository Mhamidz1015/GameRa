using Bogus.DataSets;
using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.User.UnitTests.Abstractions;
using GameRa.Modules.Users.Domain.Users;

namespace GameRa.Modules.User.UnitTests.Users;

public class UserTests : BaseTest
{
    [Fact]
    public void Create_ShouldReturnUser()
    {
        // Act
        Modules.Users.Domain.Users.User user = CreateDefaultUser();

        // Assert
        user.Should().NotBeNull();
    }

    [Fact]
    public void Create_ShouldReturnUser_WithMemberRole()
    {
        // Act
        Modules.Users.Domain.Users.User user = CreateDefaultUser();

        // Assert
        user.Roles.Single().Should().Be(Role.Member);
    }

    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenUserCreated()
    {
        // Act
        Modules.Users.Domain.Users.User user = CreateDefaultUser();

        // Assert
        UserRegisteredDomainEvent domainEvent =
            AssertDomainEventWasPublished<UserRegisteredDomainEvent>(user);

        domainEvent.UserId.Should().Be(user.Id);
    }

    [Fact]
    public void Update_ShouldRaiseDomainEvent_WhenUserUpdated()
    {
        // Arrange
        Modules.Users.Domain.Users.User user = CreateDefaultUser();

        // Act
        user.Update(Faker.Internet.UserName());

        // Assert
        UserProfileUpdatedDomainEvent domainEvent =
            AssertDomainEventWasPublished<UserProfileUpdatedDomainEvent>(user);

        domainEvent.UserId.Should().Be(user.Id);
        domainEvent.Username.Should().Be(user.Username);
    }

    [Fact]
    public void Update_ShouldNotRaiseDomainEvent_WhenUserNotUpdated()
    {
        // Arrange
        Modules.Users.Domain.Users.User user = CreateDefaultUser();

        user.ClearDomainEvents();

        // Act
        user.Update(user.Username);

        // Assert
        user.DomainEvents.Should().BeEmpty();
    }
    private Modules.Users.Domain.Users.User CreateDefaultUser() =>

        Modules.Users.Domain.Users.User.Create(
            Faker.Internet.Email(),
            Faker.Internet.UserName(),
            DateTime.UtcNow,
            Guid.NewGuid().ToString()
        );
}
