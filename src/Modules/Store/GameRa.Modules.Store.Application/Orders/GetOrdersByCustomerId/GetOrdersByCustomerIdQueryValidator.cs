using FluentValidation;

namespace GameRa.Modules.Store.Application.Orders.GetOrdersByCustomerId;

internal sealed class GetOrdersByCustomerIdQueryValidator : AbstractValidator<GetOrdersByCustomerIdQuery>
{
    public GetOrdersByCustomerIdQueryValidator()
    {
        RuleFor(q => q.CustomerId).NotEmpty();
    }
}