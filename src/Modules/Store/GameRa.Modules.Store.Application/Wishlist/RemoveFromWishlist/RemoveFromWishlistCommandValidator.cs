using FluentValidation;
using GameRa.Modules.Store.Application.Wishlist.RemoveFromWishlist;

namespace GameRa.Modules.Store.Application.Wishlists.RemoveFromWishlist;

internal sealed class RemoveFromWishlistCommandValidator : AbstractValidator<RemoveFromWishlistCommand>
{
    public RemoveFromWishlistCommandValidator()
    {
        RuleFor(c => c.CustomerId).NotEmpty();
        RuleFor(c => c.GameId).NotEmpty();
    }
}