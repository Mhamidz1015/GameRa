using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Infrastructure.Authentication;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Library.Application.Abstractions;
using GameRa.Modules.Library.Application.LibraryItems.GetUserLibrary;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace GameRa.Modules.Library.Presentation.LibraryItem
{
    internal sealed class GetLibrary : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("library/filter",async(
                    LibraryFilter libraryFilter,
                    ISender sender,
                    ClaimsPrincipal user,
                    CancellationToken cancellationToken) =>
                {
                    Result<IReadOnlyCollection<LibraryItemResponse>> result = await sender.Send(
                            new GetUserLibraryQuery(user.GetUserId(), libraryFilter),
                            cancellationToken);

                    return result.Match(Results.Ok, ApiResults.Problem);
                })
                .RequireAuthorization()
                .WithTags(Tags.Library);
        }
    }
}
