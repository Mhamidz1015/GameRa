using GameRa.Common.Application.Caching;
using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Infrastructure.Authentication;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Users.Application.Users.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;

namespace GameRa.Modules.Users.Presentation.Users;

internal sealed class GetUserProfileById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users/profile", async (ClaimsPrincipal claims, ISender sender, ICacheService cacheService) =>
        {
            Guid userId = claims.GetUserId();
            string cacheKey = $"users:{userId}";

            UserResponse? cached = await cacheService.GetAsync<UserResponse>(cacheKey);

            if (cached is not null)
                return Results.Ok(cached);

            Result<UserResponse> result = await sender.Send(new GetUserByIdQuery(userId));

            if (result.IsSuccess)
                await cacheService.SetAsync(cacheKey, result.Value, TimeSpan.FromMinutes(10));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.GetProfile)
        .WithTags(Tags.Users);
    }
}