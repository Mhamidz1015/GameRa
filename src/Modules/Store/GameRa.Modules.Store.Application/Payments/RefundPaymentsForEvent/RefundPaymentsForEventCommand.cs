using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Store.Application.Payments.RefundPaymentsForEvent;

public sealed record RefundPaymentsForEventCommand(Guid GameId) : ICommand;
