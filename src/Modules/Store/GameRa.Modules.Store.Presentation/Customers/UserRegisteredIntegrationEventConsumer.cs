using GameRa.Common.Application.Exceptions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Store.Application.Customers.CreateCustomer;
using GameRa.Modules.Users.integrationEvents;
using MassTransit;
using MediatR;

namespace GameRa.Modules.Store.Presentation.Customers;

public sealed class UserRegisteredIntegrationEventConsumer(ISender sender)
    : IConsumer<UserRegisteredIntegrationGame>
{
    public async Task Consume(ConsumeContext<UserRegisteredIntegrationGame> context)
    {
        Result result = await sender.Send(
            new CreateCustomerCommand(
                context.Message.UserId,
                context.Message.Email,
                context.Message.Username),
            context.CancellationToken);

        if (result.IsFailure)
        {
            throw new GameRaException(nameof(CreateCustomerCommand), result.Error);
        }
    }
}
