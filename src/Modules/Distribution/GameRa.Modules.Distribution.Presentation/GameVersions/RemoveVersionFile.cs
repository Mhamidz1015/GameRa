using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Distribution.Application.GameVersions.RemoveVersionFile;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Distribution.Presentation.GameVersions;

internal sealed class RemoveVersionFile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("versions/{versionId:guid}/files/{fileId:guid}", async (
                Guid versionId,
                Guid fileId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                Result result = await sender.Send(new RemoveVersionFileCommand(versionId, fileId), cancellationToken);

                return result.Match(Results.NoContent, ApiResults.Problem);
            })
            .RequireAuthorization(Permissions.ManageDistribution)
            .WithTags(Tags.Distribution);
    }
}
