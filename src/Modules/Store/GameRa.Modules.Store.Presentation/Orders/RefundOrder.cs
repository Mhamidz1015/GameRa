using System.Security.Claims;
using GameRa.Common.Application.Caching;
using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Infrastructure.Authentication;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Store.Application.Orders.RefundOrder;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Store.Presentation.Orders;

internal sealed class RefundOrder : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("orders/{id}/refund", async (
            Guid id,
            ClaimsPrincipal user,
            ISender sender,
            ICacheService cacheService) =>
        {
            Result result = await sender.Send(new RefundOrderCommand(id, user.GetUserId()));

            if (result.IsSuccess)
            {
                await cacheService.RemoveAsync($"orders:{id}");
            }

            return result.Match(Results.NoContent, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.RefundOrder)
        .WithTags(Tags.Orders);
    }
}
