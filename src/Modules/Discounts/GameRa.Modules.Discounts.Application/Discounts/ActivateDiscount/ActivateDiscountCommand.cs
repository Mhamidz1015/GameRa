using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Discounts.Application.Discounts.ActivateDiscount;

public sealed record ActivateDiscountCommand(Guid DiscountId) : ICommand;