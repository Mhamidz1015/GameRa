using GameRa.Modules.Games.Application.Games.ReleaseGame;
using GameRa.Modules.Games.Domain.Abstractions;
using GameRa.Modules.Games.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Games.Presentation.Games;

internal static class ReleaseGame
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("games/{id}/publish", async (Guid id, ISender sender) =>
        {
            Result result = await sender.Send(new ReleaseGameCommand(id));

            return result.Match(Results.NoContent, ApiResults.ApiResults.Problem);
        })
        .WithTags(Tags.Games);
    }
}
