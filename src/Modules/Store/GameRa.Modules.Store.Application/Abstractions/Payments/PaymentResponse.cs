namespace GameRa.Modules.Store.Application.Abstractions.Payments;

public sealed record PaymentResponse(Guid TransactionId, decimal Amount, string Currency);
