using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Discounts.Application.Discounts.DeactivateDiscount;

public sealed record DeactivateDiscountCommand(Guid DiscountId) : ICommand;