using GameRa.Common.Application.Caching;
using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Discounts.Application.Discounts.GetDiscount;
using GameRa.Modules.Discounts.Application.Discounts.GetDiscountByCode;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Discounts.Presentation.Discount;

internal sealed class GetDiscountByCode : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("discounts/code/{code}", async (string code, ISender sender, ICacheService cacheService) =>
        {
            string cacheKey = $"discounts:code:{code}";

            DiscountResponse? cached = await cacheService.GetAsync<DiscountResponse>(cacheKey);

            if (cached is not null)
                return Results.Ok(cached);

            Result<DiscountResponse> result = await sender.Send(new GetDiscountByCodeQuery(code));

            if (result.IsSuccess)
                await cacheService.SetAsync(cacheKey, result.Value, TimeSpan.FromMinutes(10));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Discounts);
    }
}