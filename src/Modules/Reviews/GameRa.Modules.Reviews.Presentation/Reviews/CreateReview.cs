using GameRa.Common.Application.Caching;
using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Reviews.Application.Reviews.CreateReview;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Reviews.Presentation.Reviews;

internal sealed class CreateReview : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("reviews", async (Request request, ISender sender, ICacheService cacheService) =>
        {
            var command = new CreateReviewCommand(
                request.GameId,
                request.UserId,
                request.Rating,
                request.Comment);

            Result<Guid> result = await sender.Send(command);

            if (result.IsSuccess)
            {
                await cacheService.RemoveAsync($"reviews:game:{request.GameId}");
                await cacheService.RemoveAsync($"reviews:rating:{request.GameId}");
            }

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.CreateReview)
        .WithTags(Tags.Reviews);
    }

    internal sealed class Request
    {
        public Guid GameId { get; init; }

        public Guid UserId { get; init; }

        public int Rating { get; init; }

        public string Comment { get; init; }
    }
}