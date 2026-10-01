using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Library.Application.Playtime.RecordPlaytime;

public sealed record RecordPlaytimeCommand(Guid UserId, Guid GameId, int Minutes) : ICommand;