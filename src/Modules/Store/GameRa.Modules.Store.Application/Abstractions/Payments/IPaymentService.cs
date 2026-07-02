namespace GameRa.Modules.Store.Application.Abstractions.Payments;

public interface IPaymentService
{
    Task<PaymentResponse> ChargeAsync(decimal amount, string currency);

    Task RefundAsync(Guid transactionId, decimal amount);
}
