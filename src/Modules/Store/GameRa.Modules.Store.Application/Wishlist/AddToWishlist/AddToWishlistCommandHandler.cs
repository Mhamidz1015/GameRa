using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Store.Application.Abstractions.Data;
using GameRa.Modules.Store.Domain.Wishlist;

namespace GameRa.Modules.Store.Application.Wishlist.AddToWishlist;

internal sealed class AddToWishlistCommandHandler(
    IWishlistRepository wishlistRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<AddToWishlistCommand>
{
    public async Task<Result> Handle(AddToWishlistCommand request, CancellationToken cancellationToken)
    {
        bool alreadyExists = await wishlistRepository.ExistsAsync(
            request.CustomerId, request.GameId, cancellationToken);

        if (alreadyExists)
            return Result.Failure(WishlistErrors.AlreadyInWishlist);

        WishlistItem item = WishlistItem.Create(request.CustomerId, request.GameId, DateTime.UtcNow);

        wishlistRepository.Insert(item);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}