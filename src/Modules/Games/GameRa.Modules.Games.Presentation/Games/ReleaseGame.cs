using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Games.Application.Games.ReleaseGame;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;

namespace GameRa.Modules.Games.Presentation.Games;

internal sealed class ReleaseGame : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("games/{id}/Release", async (Guid id, ISender sender) =>
        {
            Result result = await sender.Send(new ReleaseGameCommand(id));

            return result.Match(Results.NoContent, ApiResults.Problem);
        })
        .WithTags(Tags.Games);
    }
}
