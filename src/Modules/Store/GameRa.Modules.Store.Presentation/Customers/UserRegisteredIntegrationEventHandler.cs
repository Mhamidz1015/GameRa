using GameRa.Common.Application.Exceptions;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Store.Application.Customers.CreateCustomer;
using GameRa.Modules.Users.integrationEvents;
using MediatR;

namespace GameRa.Modules.Store.Presentation.Customers;

internal sealed class UserRegisteredIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<UserRegisteredIntegrationEvent>
{
    public override async Task Handle(
        UserRegisteredIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(
            new CreateCustomerCommand(
                integrationEvent.UserId,
                integrationEvent.Email,
                integrationEvent.Username),
            cancellationToken);

        if (result.IsFailure)
        {
            throw new GameRaException(nameof(CreateCustomerCommand), result.Error);
        }
    }
}
