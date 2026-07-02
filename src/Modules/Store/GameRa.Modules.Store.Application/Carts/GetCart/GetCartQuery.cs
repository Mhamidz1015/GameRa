using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Store.Application.Carts.GetCart;

public sealed record GetCartQuery(Guid CustomerId) : IQuery<Cart>;
