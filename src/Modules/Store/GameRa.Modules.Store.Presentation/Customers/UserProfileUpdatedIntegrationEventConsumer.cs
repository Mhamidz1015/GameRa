using GameRa.Common.Application.Exceptions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Store.Application.Customers.UpdateCustomer;
using GameRa.Modules.Users.integrationEvents;
using MassTransit;
using MediatR;

namespace Evently.Modules.Ticketing.Presentation.Customers;

public sealed class UserProfileUpdatedIntegrationEventConsumer(ISender sender)
    : IConsumer<UserProfileUpdatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<UserProfileUpdatedIntegrationEvent> context)
    {
        Result result = await sender.Send(
            new UpdateCustomerCommand(
                context.Message.UserId,
                context.Message.UserName),
            context.CancellationToken);

        if (result.IsFailure)
        {
            throw new GameRaException(nameof(UpdateCustomerCommand), result.Error);
        }
    }
}
