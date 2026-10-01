using GameRa.Common.Application.Caching;
using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Games.Application.Categories.ArchiveCategory;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Games.Presentation.Categories;

internal sealed class ArchiveCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("categories/{id}/archive", async (Guid id, ISender sender, ICacheService cacheService) =>
        {
            Result result = await sender.Send(new ArchiveCategoryCommand(id));

            if (result.IsSuccess)
            {
                await cacheService.RemoveAsync("categories");
                await cacheService.RemoveAsync($"categories:{id}");
            }

            return result.Match(() => Results.Ok(), ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.ArchiveCategory)
        .WithTags(Tags.Categories);
    }
}