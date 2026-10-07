using System.Security.Claims;
using GameRa.Common.Application.Caching;
using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Infrastructure.Authentication;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Library.Application.LibraryItems.ArchivedLibraryItem;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Library.Presentation.LibraryItem;

internal sealed class ArchiveLibraryItem : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("library/{gameId:guid}/archive", async (
                Guid gameId,
                ISender sender,
                ClaimsPrincipal user,
                ICacheService cacheService,
                CancellationToken cancellationToken) =>
        {
            Guid userId = user.GetUserId();

            Result result = await sender.Send(
                new ArchivedLibraryItemCommand(userId, gameId),
                cancellationToken);

            if (result.IsSuccess)
            {
                await cacheService.RemoveAsync($"favorites:{userId}", cancellationToken);
            }

            return result.Match(Results.NoContent, ApiResults.Problem);
        })
            .RequireAuthorization(Permissions.GetLibrary)
            .WithTags(Tags.Library);
    }
}