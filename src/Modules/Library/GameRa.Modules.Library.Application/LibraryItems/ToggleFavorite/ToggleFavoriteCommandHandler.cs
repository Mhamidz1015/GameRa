using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Library.Application.Abstractions.Data;
using GameRa.Modules.Library.Domain.LibraryItems;

namespace GameRa.Modules.Library.Application.LibraryItems.ToggleFavorite;

internal sealed class ToggleFavoriteCommandHandler(
    ILibraryItemRepository libraryItemRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<ToggleFavoriteCommand>
{
    public async Task<Result> Handle(ToggleFavoriteCommand request, CancellationToken cancellationToken)
    {
        LibraryItem? item = await libraryItemRepository.GetByUserAndGameAsync(
            request.UserId, request.GameId, cancellationToken);

        if (item is null)
            return Result.Failure(LibraryItemErrors.NotOwned(request.GameId));

        item.ToggleFavorite();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}