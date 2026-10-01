using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Games.Application.Games.UpdateGameRating;

public sealed record UpdateGameRatingCommand(
    Guid GameId,
    string Action,
    int NewRating,
    int OldRating = 0) : ICommand;