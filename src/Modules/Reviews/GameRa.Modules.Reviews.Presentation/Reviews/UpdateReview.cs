using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Reviews.Application.Reviews.UpdateReview;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Reviews.Presentation.Reviews;

internal sealed class UpdateReview : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("reviews/{id}", async (Guid id, Request request, ISender sender) =>
        {
            var command = new UpdateReviewCommand(
                id,
                request.UserId,
                request.Rating,
                request.Comment);

            Result result = await sender.Send(command);

            return result.Match(() => Results.Ok(), ApiResults.Problem);
        })
        .WithTags(Tags.Reviews);
    }

    internal sealed class Request
    {
        public Guid UserId { get; init; }
        public int Rating { get; init; }
        public string Comment { get; init; }
    }
}
