using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Distribution.Application.GameVersions.RegisterVersionFile;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Distribution.Presentation.GameVersions;

internal sealed class RegisterVersionFile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("versions/{versionId:guid}/files", async (
                Guid versionId,
                Request request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                Result<RegisterVersionFileResponse> result = await sender.Send(
                    new RegisterVersionFileCommand(versionId, request.RelativePath, request.SizeBytes, request.Sha256),
                    cancellationToken);

                return result.Match(Results.Ok, ApiResults.Problem);
            })
            .RequireAuthorization(Permissions.ManageDistribution)
            .WithTags(Tags.Distribution);
    }

    internal sealed class Request
    {
        public string RelativePath { get; init; } = string.Empty;

        public long SizeBytes { get; init; }

        public string Sha256 { get; init; } = string.Empty;
    }
}
