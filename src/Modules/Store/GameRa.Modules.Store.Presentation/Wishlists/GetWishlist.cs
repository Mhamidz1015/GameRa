using GameRa.Common.Application.Caching;
using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Store.Application.Wishlist.GetWishlist;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Store.Presentation.Wishlists;

internal sealed class GetWishlist : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("wishlist/{customerId}", async (
            Guid customerId, ISender sender, ICacheService cacheService) =>
        {
            string cacheKey = $"wishlist:{customerId}";

            IReadOnlyCollection<WishlistItemResponse>? cached =
                await cacheService.GetAsync<IReadOnlyCollection<WishlistItemResponse>>(cacheKey);

            if (cached is not null)
                return Results.Ok(cached);

            Result<IReadOnlyCollection<WishlistItemResponse>> result =
                await sender.Send(new GetWishlistQuery(customerId));

            if (result.IsSuccess)
                await cacheService.SetAsync(cacheKey, result.Value, TimeSpan.FromMinutes(2));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.ManageWishlist)
        .WithTags(Tags.Wishlist);
    }
}