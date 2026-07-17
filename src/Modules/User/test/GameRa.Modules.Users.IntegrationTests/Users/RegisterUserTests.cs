using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using GameRa.Modules.Users.IntegrationTests.Abstractions;
using GameRa.Modules.Users.Application.Users.RegisterUser;

namespace GameRa.Modules.Users.IntegrationTests.Users;

public class RegisterUserTests : BaseIntegrationTest
{
    public RegisterUserTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    public static readonly TheoryData<string, string, string> InvalidRequests = new()
    {
        { "", Faker.Internet.Password(), Faker.Internet.UserName() },
        { Faker.Internet.Email(), "", Faker.Internet.UserName() },
        { Faker.Internet.Email(), "12345", Faker.Internet.UserName() },
        { Faker.Internet.Email(), Faker.Internet.Password(), "" }
    };


    [Theory]
    [MemberData(nameof(InvalidRequests))]
    public async Task Should_ReturnBadRequest_WhenRequestIsNotValid(
        string email,
        string password,
        string username)
    {
        // Arrange
        var request = new Presentation.Users.RegisterUser.Request
        {
            Email = email,
            Password = password,
            Username = username
        };

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("users/register", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_ReturnOk_WhenRequestIsValid()
    {
        // Arrange
        var request = new Presentation.Users.RegisterUser.Request
        {
            Email = "create@test.com",
            Password = Faker.Internet.Password(),
            Username = Faker.Internet.UserName()
        };

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("users/register", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_ReturnAccessToken_WhenUserIsRegistered()
    {
        // Arrange
        var request = new Presentation.Users.RegisterUser.Request
        {
            Email = "token@test.com",
            Password = Faker.Internet.Password(),
            Username = Faker.Internet.UserName()
        };

        await HttpClient.PostAsJsonAsync("users/register", request);

        // Act
        string accessToken = await GetAccessTokenAsync(request.Email, request.Password);

        // Assert
        accessToken.Should().NotBeEmpty();
    }
}
