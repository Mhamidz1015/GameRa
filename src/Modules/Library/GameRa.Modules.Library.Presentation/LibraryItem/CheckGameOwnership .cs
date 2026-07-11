using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Infrastructure.Authentication;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Library.Application.LibraryItems.CheckGameOwnership;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace GameRa.Modules.Library.Presentation.LibraryItem
{
    internal sealed class CheckGameOwnership : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("library/{gameId:guid}/ownership", async (Guid gameId, ISender sender, ClaimsPrincipal user,
                    CancellationToken cancellationToken) =>
                {
                    Result<bool> result = await sender.Send( new CheckGameOwnershipQuery (user.GetUserId(), gameId),
                            cancellationToken);

                    return result.Match(Results.Ok, ApiResults.Problem);
                })
                .RequireAuthorization()
                .WithTags(Tags.Library);
        }
    }
}
