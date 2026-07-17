using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Users.Application.Users.GetUserById;
using GameRa.Modules.Users.Application.Users.RegisterUser;
using GameRa.Modules.Users.Domain.Users;
using GameRa.Modules.Users.IntegrationTests.Abstractions;

namespace GameRa.Modules.Users.IntegrationTests.Users;

public class GetUserByIdTests : BaseIntegrationTest
{
    public GetUserByIdTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnError_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        Result<UserResponse> userResult = await Sender.Send(new GetUserByIdQuery(userId));

        // Assert
        userResult.Error.Should().Be(UserErrors.NotFound(userId));
    }

    [Fact]
    public async Task Should_ReturnUser_WhenUserExists()
    {
        // Arrange
        Result<Guid> result = await Sender.Send(new RegisterUserCommand(
            Faker.Internet.Email(),
            Faker.Internet.Password(),
            Faker.Internet.UserName(),
             DateTime.UtcNow));
        Guid userId = result.Value;

        // Act
        Result<UserResponse> userResult = await Sender.Send(new GetUserByIdQuery(userId));

        // Assert
        userResult.IsSuccess.Should().BeTrue();
        userResult.Value.Should().NotBeNull();
    }
}
