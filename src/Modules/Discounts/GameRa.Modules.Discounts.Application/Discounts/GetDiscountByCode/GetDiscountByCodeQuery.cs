using GameRa.Common.Application.Messaging;
using GameRa.Modules.Discounts.Application.Discounts.GetDiscount;

namespace GameRa.Modules.Discounts.Application.Discounts.GetDiscountByCode;

public sealed record GetDiscountByCodeQuery(string Code) : IQuery<DiscountResponse>;