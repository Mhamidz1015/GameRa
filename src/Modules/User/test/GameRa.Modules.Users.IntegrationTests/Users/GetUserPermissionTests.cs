using GameRa.Common.Application.Authorization;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Users.Application.Users.GetUserPermissions;
using GameRa.Modules.Users.Domain.Users;
using GameRa.Modules.Users.IntegrationTests.Abstractions;
using FluentAssertions;
using GameRa.Modules.Users.Application.Users.RegisterUser;

namespace GameRa.Modules.Users.IntegrationTests.Users;

public class GetUserPermissionTests : BaseIntegrationTest
{
    public GetUserPermissionTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnError_WhenUserDoesNotExist()
    {
        // Arrange
        string identityId = Guid.NewGuid().ToString();

        // Act
        Result<PermissionsResponse> permissionsResult = await Sender.Send(new GetUserPermissionsQuery(identityId));

        // Assert
        permissionsResult.Error.Should().Be(UserErrors.NotFound(identityId));
    }

    [Fact]
    public async Task Should_ReturnPermissions_WhenUserExists()
    {
        // Arrange
        Result<Guid> result = await Sender.Send(new RegisterUserCommand(
            Faker.Internet.Email(),
            Faker.Internet.Password(),
            Faker.Internet.UserName(),
            DateTime.UtcNow));

        string identityId = DbContext.Users.Single(u => u.Id == result.Value).IdentityId;

        // Act
        Result<PermissionsResponse> permissionsResult = await Sender.Send(new GetUserPermissionsQuery(identityId));

        // Assert
        permissionsResult.IsSuccess.Should().BeTrue();
        permissionsResult.Value.Permissions.Should().NotBeEmpty();
    }
}
