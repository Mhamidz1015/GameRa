using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Store.Application.Abstractions.Data;
using GameRa.Modules.Store.Domain.Wishlist;

namespace GameRa.Modules.Store.Application.Wishlist.RemoveFromWishlist;

internal sealed class RemoveFromWishlistCommandHandler(
    IWishlistRepository wishlistRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RemoveFromWishlistCommand>
{
    public async Task<Result> Handle(RemoveFromWishlistCommand request, CancellationToken cancellationToken)
    {
        WishlistItem? item = await wishlistRepository.GetAsync(
            request.CustomerId, request.GameId, cancellationToken);

        if (item is null)
            return Result.Failure(WishlistErrors.NotFound);

        wishlistRepository.Remove(item);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}