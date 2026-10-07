using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Library.Application.Abstractions.Data;
using GameRa.Modules.Library.Domain.LibraryItems;

namespace GameRa.Modules.Library.Application.LibraryItems.UnarchiveLibraryItem;

internal sealed class UnarchiveLibraryItemCommandHandler(
    ILibraryItemRepository libraryItemRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UnarchiveLibraryItemCommand>
{
    public async Task<Result> Handle(
        UnarchiveLibraryItemCommand request,
        CancellationToken cancellationToken)
    {
        LibraryItem? libraryItem = await libraryItemRepository
            .GetByUserAndGameAsync(request.UserId, request.GameId, cancellationToken);

        if (libraryItem is null)
            return Result.Failure(LibraryItemErrors.NotFound);

        Result result = libraryItem.Unarchive();

        if (result.IsFailure)
            return result;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}