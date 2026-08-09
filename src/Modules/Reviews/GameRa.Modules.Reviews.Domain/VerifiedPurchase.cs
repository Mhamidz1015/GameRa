namespace GameRa.Modules.Reviews.Domain;

public sealed class VerifiedPurchase
{
    private VerifiedPurchase() { }

    public Guid Id { get; private set; }
    public Guid GameId { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime PurchasedAtUtc { get; private set; }

    public static VerifiedPurchase Create(Guid gameId, Guid userId, DateTime purchasedAtUtc)
    {
        return new VerifiedPurchase
        {
            Id = Guid.NewGuid(),
            GameId = gameId,
            UserId = userId,
            PurchasedAtUtc = purchasedAtUtc
        };
    }
}