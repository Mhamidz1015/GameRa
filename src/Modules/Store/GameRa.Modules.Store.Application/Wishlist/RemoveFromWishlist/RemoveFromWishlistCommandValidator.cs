using FluentValidation;

namespace GameRa.Modules.Store.Application.Wishlist.RemoveFromWishlist;

internal sealed class RemoveFromWishlistCommandValidator : AbstractValidator<RemoveFromWishlistCommand>
{
    public RemoveFromWishlistCommandValidator()
    {
        RuleFor(c => c.CustomerId).NotEmpty();
        RuleFor(c => c.GameId).NotEmpty();
    }
}