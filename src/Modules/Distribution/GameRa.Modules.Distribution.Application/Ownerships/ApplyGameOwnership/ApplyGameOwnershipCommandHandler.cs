using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Distribution.Application.Abstractions.Data;
using GameRa.Modules.Distribution.Domain.Ownerships;

namespace GameRa.Modules.Distribution.Application.Ownerships.ApplyGameOwnership;

internal sealed class ApplyGameOwnershipCommandHandler(
    IGameOwnershipRepository gameOwnershipRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<ApplyGameOwnershipCommand>
{
    public async Task<Result> Handle(ApplyGameOwnershipCommand request, CancellationToken cancellationToken)
    {
        DateTime occurredOnUtc = request.OccurredOnUtc.Kind == DateTimeKind.Local
            ? request.OccurredOnUtc.ToUniversalTime()
            : DateTime.SpecifyKind(request.OccurredOnUtc, DateTimeKind.Utc);

        GameOwnership? ownership = await gameOwnershipRepository.GetAsync(
            request.UserId,
            request.GameId,
            cancellationToken);

        if (ownership is null)
        {
            gameOwnershipRepository.Insert(
                GameOwnership.Create(request.UserId, request.GameId, request.Owned, occurredOnUtc));
        }
        else if (!ownership.Apply(request.Owned, occurredOnUtc))
        {
            return Result.Success();
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
