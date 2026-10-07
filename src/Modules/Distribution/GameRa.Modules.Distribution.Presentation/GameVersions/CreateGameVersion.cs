using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Distribution.Application.GameVersions.CreateGameVersion;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Distribution.Presentation.GameVersions;

internal sealed class CreateGameVersion : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("games/{gameId:guid}/versions", async (
                Guid gameId,
                Request request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                Result<Guid> result = await sender.Send(
                    new CreateGameVersionCommand(gameId, request.Version, request.ReleaseNotes),
                    cancellationToken);

                return result.Match(Results.Ok, ApiResults.Problem);
            })
            .RequireAuthorization(Permissions.ManageDistribution)
            .WithTags(Tags.Distribution);
    }

    internal sealed class Request
    {
        public string Version { get; init; } = string.Empty;

        public string? ReleaseNotes { get; init; }
    }
}
