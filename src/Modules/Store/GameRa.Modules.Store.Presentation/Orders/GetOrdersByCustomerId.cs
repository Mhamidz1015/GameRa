using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Store.Application.Orders.GetOrdersByCustomerId;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Store.Presentation.Orders;

internal sealed class GetOrdersByCustomerId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("orders/customer/{customerId}", async (Guid customerId, ISender sender) =>
        {
            Result<IReadOnlyCollection<OrderSummaryResponse>> result =
                await sender.Send(new GetOrdersByCustomerIdQuery(customerId));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.GetOrders)
        .WithTags(Tags.Orders);
    }
}