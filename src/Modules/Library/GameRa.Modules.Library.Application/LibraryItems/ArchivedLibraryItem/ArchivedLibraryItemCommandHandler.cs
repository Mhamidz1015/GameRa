using GameRa.Common.Application.Data;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Library.Application.Abstractions.Data;
using GameRa.Modules.Library.Domain.LibraryItems;

namespace GameRa.Modules.Library.Application.LibraryItems.ArchivedLibraryItem;

internal sealed class ArchiveLibraryItemCommandHandler(
    ILibraryItemRepository libraryItemRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<ArchivedLibraryItemCommand>
{
    public async Task<Result> Handle(
        ArchivedLibraryItemCommand request,
        CancellationToken cancellationToken)
    {
        LibraryItem? libraryItem = await libraryItemRepository
            .GetByUserAndGameAsync(request.UserId, request.GameId, cancellationToken);

        if (libraryItem is null)
            return Result.Failure(LibraryItemErrors.NotFound);

        Result result = libraryItem.Archive();

        if (result.IsFailure)
            return result;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
