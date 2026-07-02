using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Store.Application.Carts;

public static class CartErrors
{
    public static readonly Error Empty = Error.Problem("Carts.Empty", "The cart is empty");
}
