using GameRa.Common.Application.Exceptions;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Users.Application.Users.GetUserById;
using GameRa.Modules.Users.Domain.Users;
using GameRa.Modules.Users.IntegrationEvents;
using MediatR;

namespace GameRa.Modules.Users.Application.Users.RegisterUser;

internal sealed class UserRegisteredDomainEventHandler(ISender sender, IEventBus eventBus)
    : DomainEventHandler<UserRegisteredDomainEvent>
{
    public override async Task Handle
        (UserRegisteredDomainEvent notification,
        CancellationToken cancellationToken = default)
    {
        Result<UserResponse> result = await sender.Send(new GetUserByIdQuery(notification.UserId), cancellationToken);

        if (result.IsFailure)
        {
            throw new GameRaException(nameof(GetUserByIdQuery), result.Error);
        }

        await eventBus.PublishAsync(
            new UserRegisteredIntegrationEvent(
                notification.Id,
                notification.OccurredOnUtc,
                result.Value.Id,
                result.Value.CreatedOnUtc,
                result.Value.Email,
                result.Value.Username),
            cancellationToken);
    }
}
