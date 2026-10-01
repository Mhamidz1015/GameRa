using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Library.Application.LibraryItems.ToggleFavorite;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Library.Presentation.LibraryItem;

internal sealed class ToggleFavorite : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("library/favorite", async (Request request, ISender sender) =>
        {
            Result result = await sender.Send(
                new ToggleFavoriteCommand(request.UserId, request.GameId));

            return result.Match(() => Results.Ok(), ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.GetLibrary)
        .WithTags(Tags.Library);
    }

    internal sealed class Request
    {
        public Guid UserId { get; init; }
        public Guid GameId { get; init; }
    }
}