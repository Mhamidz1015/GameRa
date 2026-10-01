using GameRa.Common.Application.Caching;
using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Games.Application.Games.SearchGames;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Games.Presentation.Games;

internal sealed class SearchGames : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("games/search", async (
            ISender sender,
            ICacheService cacheService,
            string? searchTerm,
            Guid? catagoryId,
            int page = 1,
            int pageSize = 15) =>
        {
            string cacheKey = $"games:search:{searchTerm}:{catagoryId}:{page}:{pageSize}";

            SearchGamesResponse? cached = await cacheService.GetAsync<SearchGamesResponse>(cacheKey);

            if (cached is not null)
                return Results.Ok(cached);

            Result<SearchGamesResponse> result = await sender.Send(
                new SearchGamesQuery(catagoryId, searchTerm, page, pageSize));

            if (result.IsSuccess)
                await cacheService.SetAsync(cacheKey, result.Value, TimeSpan.FromMinutes(5));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Games);
    }
}