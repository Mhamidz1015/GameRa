using GameRa.Common.Application.Caching;
using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Users.Application.Users.UpdateUser;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Users.Presentation.Users;

internal sealed class UpdateUserProfile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("users/{id}/profile", async (Guid id, Request request, ISender sender, ICacheService cacheService) =>
        {
            Result result = await sender.Send(new UpdateUserCommand(id, request.Username));

            if (result.IsSuccess)
                await cacheService.RemoveAsync($"users:{id}");

            return result.Match(Results.NoContent, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.UpdateProfile)
        .WithTags(Tags.Users);
    }

    internal sealed class Request
    {
        public string Username { get; init; }
    }
}