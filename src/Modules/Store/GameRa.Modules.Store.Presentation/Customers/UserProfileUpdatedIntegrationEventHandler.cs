using GameRa.Common.Application.Exceptions;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Store.Application.Customers.UpdateCustomer;
using GameRa.Modules.Users.IntegrationEvents;
using MediatR;

namespace GameRa.Modules.Store.Presentation.Customers;

internal sealed class UserProfileUpdatedIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<UserProfileUpdatedIntegrationEvent>
{
    public override async Task Handle(
        UserProfileUpdatedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(
            new UpdateCustomerCommand(
                integrationEvent.UserId,
                integrationEvent.UserName),
            cancellationToken);

        if (result.IsFailure)
        {
            throw new GameRaException(nameof(UpdateCustomerCommand), result.Error);
        }
    }
}
