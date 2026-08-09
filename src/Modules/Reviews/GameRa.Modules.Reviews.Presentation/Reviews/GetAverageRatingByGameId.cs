using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Reviews.Application.Reviews.GetAverageRatingByGameId;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Reviews.Presentation.Reviews;

internal sealed class GetAverageRatingByGameId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("reviews/game/{gameId}/average-rating", async (Guid gameId, ISender sender) =>
        {
            Result<AverageRatingResponse> result =
                await sender.Send(new GetAverageRatingByGameIdQuery(gameId));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Reviews);
    }
}
