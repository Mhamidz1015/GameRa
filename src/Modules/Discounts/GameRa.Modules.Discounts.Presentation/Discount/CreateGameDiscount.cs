using GameRa.Common.Application.Caching;
using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Discounts.Application.Discounts.CreateGameDiscount;
using GameRa.Modules.Discounts.Domain;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Discounts.Presentation.Discount;

internal sealed class CreateGameDiscount : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("discounts/game", async (Request request, ISender sender, ICacheService cacheService) =>
        {
            var command = new CreateGameDiscountCommand(
                request.Code,
                request.Type,
                request.Amount,
                request.GameId,
                request.StartDateTimeUtc,
                request.EndDateTimeUtc);

            Result<Guid> result = await sender.Send(command);

            if (result.IsSuccess)
                await cacheService.RemoveAsync($"discounts:game:{request.GameId}:*");

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.CreateDiscount)
        .WithTags(Tags.Discounts);
    }

    internal sealed class Request
    {
        public string Code { get; init; }

        public DiscountType Type { get; init; }

        public decimal Amount { get; init; }

        public Guid GameId { get; init; }

        public DateTime StartDateTimeUtc { get; init; }

        public DateTime EndDateTimeUtc { get; init; }
    }
}