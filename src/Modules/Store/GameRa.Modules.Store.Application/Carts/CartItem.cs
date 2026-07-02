namespace GameRa.Modules.Store.Application.Carts;

public sealed class CartItem
{
    public Guid GameId { get; set; }

    public decimal Price { get; set; }

    public string Currency { get; set; }
}
