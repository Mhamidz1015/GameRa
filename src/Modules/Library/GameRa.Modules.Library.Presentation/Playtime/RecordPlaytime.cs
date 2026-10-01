using GameRa.Common.Application.Caching;
using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Library.Application.Playtime.RecordPlaytime;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Library.Presentation.Playtime;

internal sealed class RecordPlaytime : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("library/playtime", async (Request request, ISender sender, ICacheService cacheService) =>
        {
            Result result = await sender.Send(
                new RecordPlaytimeCommand(request.UserId, request.GameId, request.Minutes));

            if (result.IsSuccess)
                await cacheService.RemoveAsync($"playtime:{request.UserId}");

            return result.Match(() => Results.Ok(), ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.GetLibrary)
        .WithTags(Tags.Library);
    }

    internal sealed class Request
    {
        public Guid UserId { get; init; }
        public Guid GameId { get; init; }
        public int Minutes { get; init; }
    }
}