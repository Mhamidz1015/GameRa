using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Store.Application.Payments.RefundPayment;

public sealed record RefundPaymentCommand(Guid PaymentId, decimal Amount) : ICommand;
