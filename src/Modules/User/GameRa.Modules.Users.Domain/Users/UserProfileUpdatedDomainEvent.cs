using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Users.Domain.Users;

public sealed class UserProfileUpdatedDomainEvent(Guid userId, string username) : DomainEvent
{
    public Guid UserId { get; init; } = userId;

    public string Username { get; init; } = username;
}
