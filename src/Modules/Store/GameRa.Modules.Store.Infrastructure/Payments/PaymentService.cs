using GameRa.Modules.Store.Application.Abstractions.Payments;

namespace GameRa.Modules.Store.Infrastructure.Payments;

internal sealed class PaymentService : IPaymentService
{
    public Task<PaymentResponse> ChargeAsync(decimal amount)
    {
        return Task.FromResult(new PaymentResponse(Guid.NewGuid(), amount));
    }

    public Task RefundAsync(Guid transactionId, decimal amount)
    {
        return Task.CompletedTask;
    }
}
