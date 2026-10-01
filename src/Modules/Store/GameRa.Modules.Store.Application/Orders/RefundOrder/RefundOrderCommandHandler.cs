using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Store.Application.Abstractions.Data;
using GameRa.Modules.Store.Domain.Orders;

namespace GameRa.Modules.Store.Application.Orders.RefundOrder;

internal sealed class RefundOrderCommandHandler(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RefundOrderCommand>
{
    public async Task<Result> Handle(RefundOrderCommand request, CancellationToken cancellationToken)
    {
        Order? order = await orderRepository.GetAsync(request.OrderId, cancellationToken);

        if (order is null)
        {
            return Result.Failure(OrderErrors.NotFound(request.OrderId));
        }

        if (order.CustomerId != request.CustomerId)
        {
            return Result.Failure(OrderErrors.Forbidden);
        }

        Result result = order.Refund();

        if (result.IsFailure)
        {
            return result;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
