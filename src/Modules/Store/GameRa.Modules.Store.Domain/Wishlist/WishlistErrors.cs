using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Store.Domain.Wishlist
{
    public static class WishlistErrors
    {
        public static readonly Error AlreadyInWishlist = Error.Conflict(
            "Wishlist.AlreadyExists",
            "This game is already in the wishlist");

        public static readonly Error NotFound = Error.NotFound(
            "Wishlist.NotFound",
            "Wishlist item was not found");
    }
}