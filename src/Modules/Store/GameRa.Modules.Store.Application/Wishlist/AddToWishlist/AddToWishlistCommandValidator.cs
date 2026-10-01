using FluentValidation;

namespace GameRa.Modules.Store.Application.Wishlist.AddToWishlist;

internal sealed class AddToWishlistCommandValidator : AbstractValidator<AddToWishlistCommand>
{
    public AddToWishlistCommandValidator()
    {
        RuleFor(c => c.CustomerId).NotEmpty();
        RuleFor(c => c.GameId).NotEmpty();
    }
}