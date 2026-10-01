using GameRa.Common.Application.Caching;
using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Games.Application.Games.DelistGame;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Games.Presentation.Games;

internal sealed class DelistGame : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("Games/{id}/delist", async (Guid id, ISender sender, ICacheService cacheService) =>
        {
            Result result = await sender.Send(new DelistGameCommand(id));

            if (result.IsSuccess)
            {
                await cacheService.RemoveAsync("games");
                await cacheService.RemoveAsync($"games:{id}");
            }

            return result.Match(Results.NoContent, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.DelistGame)
        .WithTags(Tags.Games);
    }
}