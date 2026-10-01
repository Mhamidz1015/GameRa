using GameRa.Common.Application.Caching;
using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Library.Application.Playtime.GetPlaytimeSummary;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Library.Presentation.Playtime;

internal sealed class GetPlaytimeSummary : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("library/playtime/{userId}", async (
            Guid userId, ISender sender, ICacheService cacheService) =>
        {
            string cacheKey = $"playtime:{userId}";

            IReadOnlyCollection<PlaytimeSummaryResponse>? cached =
                await cacheService.GetAsync<IReadOnlyCollection<PlaytimeSummaryResponse>>(cacheKey);

            if (cached is not null)
                return Results.Ok(cached);

            Result<IReadOnlyCollection<PlaytimeSummaryResponse>> result =
                await sender.Send(new GetPlaytimeSummaryQuery(userId));

            if (result.IsSuccess)
                await cacheService.SetAsync(cacheKey, result.Value, TimeSpan.FromMinutes(5));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.GetLibrary)
        .WithTags(Tags.Library);
    }
}