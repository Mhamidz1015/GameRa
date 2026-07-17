using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Users.Application.Users.RegisterUser;
using GameRa.Modules.Users.Application.Users.UpdateUser;
using GameRa.Modules.Users.Domain.Users;
using GameRa.Modules.Users.IntegrationTests.Abstractions;

namespace GameRa.Modules.Users.IntegrationTests.Users;

public class UpdateUserTests : BaseIntegrationTest
{
    public UpdateUserTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    public static readonly TheoryData<UpdateUserCommand> InvalidCommands = new()
    {
        new UpdateUserCommand(Guid.Empty, Faker.Internet.UserName()),
        new UpdateUserCommand(Guid.NewGuid(), Faker.Internet.UserName())
    };

    [Theory]
    [MemberData(nameof(InvalidCommands))]
    public async Task Should_ReturnError_WhenCommandIsNotValid(UpdateUserCommand command)
    {
        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task Should_ReturnError_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        Result updateResult = await Sender.Send(
            new UpdateUserCommand(userId, Faker.Internet.UserName()));

        // Assert
        updateResult.Error.Should().Be(UserErrors.NotFound(userId));
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenUserExists()
    {
        // Arrange
        Result<Guid> result = await Sender.Send(new RegisterUserCommand(
            Faker.Internet.Email(),
            Faker.Internet.Password(),
            Faker.Internet.UserName(),
             DateTime.UtcNow));

        Guid userId = result.Value;

        // Act
        Result updateResult = await Sender.Send(
            new UpdateUserCommand(userId, Faker.Internet.UserName()));

        // Assert
        updateResult.IsSuccess.Should().BeTrue();
    }
}
