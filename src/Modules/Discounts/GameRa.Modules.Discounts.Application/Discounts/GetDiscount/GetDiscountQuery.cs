using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Discounts.Application.Discounts.GetDiscount;

public sealed record GetDiscountQuery(Guid DiscountId) : IQuery<DiscountResponse>;