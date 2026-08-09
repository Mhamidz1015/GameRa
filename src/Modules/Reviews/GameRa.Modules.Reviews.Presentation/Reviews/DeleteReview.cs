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
        app.MapDelete("reviews/{id}", async (Guid id, Guid userId, ISender sender) =>
        {
            Result result = await sender.Send(new DeleteReviewCommand(id, userId));

            return result.Match(() => Results.NoContent(), ApiResults.Problem);
        })
        .WithTags(Tags.Reviews);
    }
}
