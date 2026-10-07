using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Distribution.Application.GameVersions.ConfirmVersionFileUpload;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Distribution.Presentation.GameVersions;

internal sealed class ConfirmVersionFileUpload : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("versions/{versionId:guid}/files/{fileId:guid}/confirm", async (
                Guid versionId,
                Guid fileId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                Result result = await sender.Send(new ConfirmVersionFileUploadCommand(versionId, fileId), cancellationToken);

                return result.Match(Results.NoContent, ApiResults.Problem);
            })
            .RequireAuthorization(Permissions.ManageDistribution)
            .WithTags(Tags.Distribution);
    }
}
