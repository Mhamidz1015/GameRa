using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Library.Application.Abstractions.Data;
using GameRa.Modules.Library.Domain.LibraryItems;

namespace GameRa.Modules.Library.Application.LibraryItems.RemoveGameFromLibrary;

internal sealed class RemoveGameFromLibraryCommandHandler(
    ILibraryItemRepository libraryItemRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RemoveGameFromLibraryCommand>
{
    public async Task<Result> Handle(
        RemoveGameFromLibraryCommand request,
        CancellationToken cancellationToken)
    {
        LibraryItem? item = await libraryItemRepository.GetByUserAndGameAsync(
            request.UserId,
            request.GameId,
            cancellationToken);

        if (item is null)
        {
            return Result.Success();
        }

        libraryItemRepository.Remove(item);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
