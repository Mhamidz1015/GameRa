using GameRa.Common.Application.Caching;
using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Discounts.Application.Discounts.GetDiscount;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Discounts.Presentation.Discount;

internal sealed class GetDiscount : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("discounts/{id}", async (Guid id, ISender sender, ICacheService cacheService) =>
        {
            string cacheKey = $"discounts:{id}";

            DiscountResponse? cached = await cacheService.GetAsync<DiscountResponse>(cacheKey);

            if (cached is not null)
                return Results.Ok(cached);

            Result<DiscountResponse> result = await sender.Send(new GetDiscountQuery(id));

            if (result.IsSuccess)
                await cacheService.SetAsync(cacheKey, result.Value, TimeSpan.FromMinutes(10));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Discounts);
    }
}