using GameRa.Common.Application.Caching;
using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Reviews.Application.Reviews.GetReview;
using GameRa.Modules.Reviews.Application.Reviews.GetReviewsByGameId;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Reviews.Presentation.Reviews;

internal sealed class GetReviewsByGameId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("reviews/game/{gameId}", async (Guid gameId, ISender sender, ICacheService cacheService) =>
        {
            string cacheKey = $"reviews:game:{gameId}";

            IReadOnlyCollection<ReviewResponse>? cached =
                await cacheService.GetAsync<IReadOnlyCollection<ReviewResponse>>(cacheKey);

            if (cached is not null)
                return Results.Ok(cached);

            Result<IReadOnlyCollection<ReviewResponse>> result =
                await sender.Send(new GetReviewsByGameIdQuery(gameId));

            if (result.IsSuccess)
                await cacheService.SetAsync(cacheKey, result.Value, TimeSpan.FromMinutes(2));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Reviews);
    }
}