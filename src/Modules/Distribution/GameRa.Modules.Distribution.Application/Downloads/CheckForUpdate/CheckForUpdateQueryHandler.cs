using FluentValidation;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Distribution.Application.Abstractions.Validation;
using GameRa.Modules.Distribution.Application.Downloads.GetLatestVersion;
using GameRa.Modules.Distribution.Domain.GameVersions;
using MediatR;

namespace GameRa.Modules.Distribution.Application.Downloads.CheckForUpdate;

internal sealed class CheckForUpdateQueryHandler(ISender sender, IValidator<CheckForUpdateQuery> validator)
    : IQueryHandler<CheckForUpdateQuery, CheckForUpdateResponse>
{
    public async Task<Result<CheckForUpdateResponse>> Handle(
        CheckForUpdateQuery request,
        CancellationToken cancellationToken)
    {
        if (await validator.ValidateRequestAsync(request, cancellationToken) is { } validationError)
        {
            return Result.Failure<CheckForUpdateResponse>(validationError);
        }

        VersionNumber.TryParse(request.From, out VersionNumber current);

        Result<LatestVersionResponse> latest = await sender.Send(
            new GetLatestVersionQuery(request.UserId, request.GameId),
            cancellationToken);

        if (latest.IsFailure)
        {
            return Result.Failure<CheckForUpdateResponse>(latest.Error);
        }

        VersionNumber.TryParse(latest.Value.Version, out VersionNumber latestNumber);

        return new CheckForUpdateResponse(
            latestNumber > current,
            current.ToString(),
            latestNumber.ToString(),
            latest.Value.VersionId);
    }
}
