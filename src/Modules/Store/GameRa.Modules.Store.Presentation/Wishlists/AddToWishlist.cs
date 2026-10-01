using GameRa.Common.Application.Caching;
using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Store.Application.Wishlist.AddToWishlist;
using GameRa.Modules.Store.Application.Wishlists.AddToWishlist;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Store.Presentation.Wishlists;

internal sealed class AddToWishlist : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("wishlist", async (Request request, ISender sender, ICacheService cacheService) =>
        {
            Result result = await sender.Send(
                new AddToWishlistCommand(request.CustomerId, request.GameId));

            if (result.IsSuccess)
                await cacheService.RemoveAsync($"wishlist:{request.CustomerId}");

            return result.Match(() => Results.Ok(), ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.ManageWishlist)
        .WithTags(Tags.Wishlist);
    }

    internal sealed class Request
    {
        public Guid CustomerId { get; init; }
        public Guid GameId { get; init; }
    }
}