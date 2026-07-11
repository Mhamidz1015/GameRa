using GameRa.Common.Application.Messaging;
using GameRa.Modules.Library.Application.LibraryItems.GetUserLibrary;

namespace GameRa.Modules.Library.Application.LibraryItems.CheckGameOwnership;

public sealed record CheckGameOwnershipQuery(Guid UserId, Guid GameId) : IQuery<bool>;
