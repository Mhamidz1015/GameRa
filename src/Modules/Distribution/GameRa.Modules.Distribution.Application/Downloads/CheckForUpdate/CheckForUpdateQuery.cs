using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Distribution.Application.Downloads.CheckForUpdate;

public sealed record CheckForUpdateQuery(Guid UserId, Guid GameId, string From) : IQuery<CheckForUpdateResponse>;
