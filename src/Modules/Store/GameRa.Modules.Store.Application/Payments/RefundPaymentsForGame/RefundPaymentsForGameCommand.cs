using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Store.Application.Payments.RefundPaymentsForGame;

public sealed record RefundPaymentsForGameCommand(Guid GameId) : ICommand;
