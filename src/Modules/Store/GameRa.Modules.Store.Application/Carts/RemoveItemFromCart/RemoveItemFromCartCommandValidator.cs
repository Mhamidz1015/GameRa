using FluentValidation;

namespace GameRa.Modules.Store.Application.Carts.RemoveItemFromCart;

internal sealed class RemoveItemFromCartCommandValidator : AbstractValidator<RemoveItemFromCartCommand>
{
    public RemoveItemFromCartCommandValidator()
    {
        RuleFor(c => c.CustomerId).NotEmpty();
        RuleFor(c => c.GameId).NotEmpty();
    }
}
