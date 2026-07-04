using GameRa.Common.Application.MessagingGameBus;

namespace GameRa.Modules.Users.integrationEvents;

public sealed class UserProfileUpdatedIntegrationEvent : IntegrationEvent
{
    public UserProfileUpdatedIntegrationEvent(
        Guid id,
        DateTime occurredOnUtc,
        Guid userId,
        string Username)
        : base(id, occurredOnUtc)
    {
        UserId = userId;
        UserName = Username;
    }

    public Guid UserId { get; init; }

    public string UserName { get; init; }
}
