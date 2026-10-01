using GameRa.Common.Application.Caching;
using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Games.Application.Games.ReleaseGame;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Games.Presentation.Games;

internal sealed class ReleaseGame : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("games/{id}/Release", async (Guid id, ISender sender, ICacheService cacheService) =>
        {
            Result result = await sender.Send(new ReleaseGameCommand(id));

            if (result.IsSuccess)
            {
                await cacheService.RemoveAsync("games");
                await cacheService.RemoveAsync($"games:{id}");
            }

            return result.Match(Results.NoContent, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.ReleaseGame)
        .WithTags(Tags.Games);
    }
}