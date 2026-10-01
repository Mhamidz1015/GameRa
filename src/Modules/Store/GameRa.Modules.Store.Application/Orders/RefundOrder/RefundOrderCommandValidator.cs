using FluentValidation;

namespace GameRa.Modules.Store.Application.Orders.RefundOrder;

internal sealed class RefundOrderCommandValidator : AbstractValidator<RefundOrderCommand>
{
    public RefundOrderCommandValidator()
    {
        RuleFor(c => c.OrderId).NotEmpty();
        RuleFor(c => c.CustomerId).NotEmpty();
    }
}
