namespace GameRa.Modules.Store.Domain.Wishlist;

public sealed class WishlistItem
{
    private WishlistItem() { }

    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid GameId { get; private set; }
    public DateTime AddedAtUtc { get; private set; }

    public static WishlistItem Create(Guid customerId, Guid gameId, DateTime addedAtUtc)
    {
        return new WishlistItem
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            GameId = gameId,
            AddedAtUtc = addedAtUtc
        };
    }
}