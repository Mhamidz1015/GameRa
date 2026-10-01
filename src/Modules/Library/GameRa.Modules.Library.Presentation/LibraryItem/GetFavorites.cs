using GameRa.Common.Application.Caching;
using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Library.Application.LibraryItems.GetFavorites;
using GameRa.Modules.Library.Application.LibraryItems.GetUserLibrary;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Library.Presentation.LibraryItem;

internal sealed class GetFavorites : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("library/favorites/{userId}", async (
            Guid userId, ISender sender, ICacheService cacheService) =>
        {
            string cacheKey = $"favorites:{userId}";

            IReadOnlyCollection<LibraryItemResponse>? cached =
                await cacheService.GetAsync<IReadOnlyCollection<LibraryItemResponse>>(cacheKey);

            if (cached is not null)
                return Results.Ok(cached);

            Result<IReadOnlyCollection<LibraryItemResponse>> result =
                await sender.Send(new GetFavoritesQuery(userId));

            if (result.IsSuccess)
                await cacheService.SetAsync(cacheKey, result.Value, TimeSpan.FromMinutes(5));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.GetLibrary)
        .WithTags(Tags.Library);
    }
}