using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Distribution.Application.GameVersions.DeprecateGameVersion;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Distribution.Presentation.GameVersions;

internal sealed class DeprecateGameVersion : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("versions/{versionId:guid}/deprecate", async (
                Guid versionId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                Result result = await sender.Send(new DeprecateGameVersionCommand(versionId), cancellationToken);

                return result.Match(Results.NoContent, ApiResults.Problem);
            })
            .RequireAuthorization(Permissions.ManageDistribution)
            .WithTags(Tags.Distribution);
    }
}
