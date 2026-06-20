using GameRa.Modules.Games.Application.Games.GetGames;
using GameRa.Modules.Games.Domain.Abstractions;
using GameRa.Modules.Games.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Games.Presentation.Games;

internal static class GetGames
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("games", async (ISender sender) =>
        {
            Result<IReadOnlyCollection<GameResponse>> result = await sender.Send(new GetGamesQuery());

            return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
        })
        .WithTags(Tags.Games);
    }
}
