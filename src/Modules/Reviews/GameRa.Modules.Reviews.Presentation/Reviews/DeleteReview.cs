using GameRa.Common.Application.Caching;
using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Reviews.Application.Reviews.DeleteReview;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Reviews.Presentation.Reviews;

internal sealed class DeleteReview : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("reviews/{id}", async (Guid id, Guid userId, Guid gameId, ISender sender, ICacheService cacheService) =>
        {
            Result result = await sender.Send(new DeleteReviewCommand(id, userId));

            if (result.IsSuccess)
            {
                await cacheService.RemoveAsync($"reviews:game:{gameId}");
                await cacheService.RemoveAsync($"reviews:rating:{gameId}");
            }

            return result.Match(() => Results.NoContent(), ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.DeleteReview)
        .WithTags(Tags.Reviews);
    }
}