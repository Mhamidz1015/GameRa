using FluentValidation;

namespace GameRa.Modules.Store.Application.Carts.RemoveGameFromAllCarts;

internal sealed class RemoveGameFromAllCartsCommandValidator : AbstractValidator<RemoveGameFromAllCartsCommand>
{
    public RemoveGameFromAllCartsCommandValidator()
    {
        RuleFor(c => c.GameId).NotEmpty();
    }
}