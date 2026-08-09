using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Discounts.Application.Discounts.GetDiscount;
using GameRa.Modules.Discounts.Application.Discounts.GetActiveDiscountsForGame;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Discounts.Presentation.Discount;

internal sealed class GetActiveDiscountsForGame : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("discounts/game/{gameId}/category/{categoryId}", async (
            Guid gameId,
            Guid categoryId,
            ISender sender) =>
        {
            Result<IReadOnlyCollection<DiscountResponse>> result =
                await sender.Send(new GetActiveDiscountsForGameQuery(gameId, categoryId));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Discounts);
    }
}
