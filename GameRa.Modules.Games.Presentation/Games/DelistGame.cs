using GameRa.Modules.Games.Domain.Abstractions;
using GameRa.Modules.Games.Presentation.ApiResults;
using GameRa.Modules.Games.Application.Games.DelistGame;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Games.Presentation.Games;

internal static class DelistGame
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("Games/{id}/cancel", async (Guid id, ISender sender) =>
        {
            Result result = await sender.Send(new DelistGameCommand(id));

            return result.Match(Results.NoContent, ApiResults.ApiResults.Problem);
        })
        .WithTags(Tags.Games);
    }
}
