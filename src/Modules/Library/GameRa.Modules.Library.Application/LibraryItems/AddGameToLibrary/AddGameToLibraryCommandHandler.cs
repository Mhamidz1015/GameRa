using GameRa.Common.Application.Clock;
using GameRa.Common.Application.Data;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Library.Application.Abstractions.Data;
using GameRa.Modules.Library.Domain.LibraryItems;

namespace GameRa.Modules.Library.Application.LibraryItems.AddGameToLibrary;

internal sealed class AddGameToLibraryCommandHandler(
    ILibraryItemRepository libraryItemRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<AddGameToLibraryCommand>
{
    public async Task<Result> Handle(AddGameToLibraryCommand request, CancellationToken cancellationToken)
    {
        bool exists = await libraryItemRepository
           .ExistsAsync(request.UserId, request.GameId, cancellationToken);

        if (exists)
            return Result.Success();

        var libraryItem = LibraryItem.Create(
            request.UserId,
            request.GameId,
            request.GameTitleSnapshot);

        libraryItemRepository.Insert(libraryItem);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
