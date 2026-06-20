using GameRa.Modules.Games.Application.Games.SearchGames;
using GameRa.Modules.Games.Domain.Abstractions;
using GameRa.Modules.Games.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Games.Presentation.Games;

internal static class SearchGames
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("games/search", async (
            ISender sender,
            string? searchTerm,
            Guid? catagoryId,
            int page = 1,
            int pageSize = 15) =>
        {
            Result<SearchGamesResponse> result = await sender.Send(
                new SearchGamesQuery( catagoryId,searchTerm, page, pageSize));

            return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
        })
        .WithTags(Tags.Games);
    }
}