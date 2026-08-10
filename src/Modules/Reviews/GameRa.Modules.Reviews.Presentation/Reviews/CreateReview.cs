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
        app.MapPost("reviews", async (Request request, ISender sender) =>
        {
            var command = new CreateReviewCommand(
                request.GameId,
                request.UserId,
                request.Rating,
                request.Comment,
                request.VerifiedPurchase);

            Result<Guid> result = await sender.Send(command);

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Reviews);
    }

    internal sealed class Request
    {
        public Guid GameId { get; init; }

        public Guid UserId { get; init; }

        public int Rating { get; init; }

        public string Comment { get; init; }

        public bool VerifiedPurchase { get; init; }
    }
}
