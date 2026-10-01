using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Store.Application.Wishlist.RemoveFromWishlist;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Store.Presentation.Wishlists;

internal sealed class RemoveFromWishlist : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("wishlist/{gameId}", async (Guid customerId, Guid gameId, ISender sender) =>
        {
            Result result = await sender.Send(
                new RemoveFromWishlistCommand(customerId, gameId));

            return result.Match(() => Results.NoContent(), ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.ManageWishlist)
        .WithTags(Tags.Wishlist);
    }
}