using GameRa.Common.Application.MessagingGameBus;

namespace GameRa.Modules.Users.integrationEvents;

public sealed class UserRegisteredIntegrationGame : IntegrationEvent
{
    public UserRegisteredIntegrationGame(
        Guid id,
        DateTime occurredOnUtc,
        Guid userId,
        DateTime createdOnUtc,
        string email,
        string username)
        : base(id, occurredOnUtc)
    {
        UserId = userId;
        Email = email;
        Username = username;
        CreatedOnUtc = createdOnUtc;
    }

    public Guid UserId { get; init; }

    public string Email { get; init; }

    public string Username { get; init; }

    public DateTime CreatedOnUtc { get; init; }
}
