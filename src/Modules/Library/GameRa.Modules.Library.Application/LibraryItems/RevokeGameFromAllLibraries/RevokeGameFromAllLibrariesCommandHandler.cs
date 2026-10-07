using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Library.Application.Abstractions.Data;
using GameRa.Modules.Library.Domain.LibraryItems;

namespace GameRa.Modules.Library.Application.LibraryItems.RevokeGameFromAllLibraries;

internal sealed class RevokeGameFromAllLibrariesCommandHandler(
    ILibraryItemRepository libraryItemRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RevokeGameFromAllLibrariesCommand>
{
    public async Task<Result> Handle(
        RevokeGameFromAllLibrariesCommand request,
        CancellationToken cancellationToken)
    {
        List<LibraryItem> items = await libraryItemRepository
            .GetByGameAsync(request.GameId, cancellationToken);

        if (items.Count == 0)
            return Result.Success();

        foreach (LibraryItem item in items)
        {
            item.Remove();
            libraryItemRepository.Remove(item);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}