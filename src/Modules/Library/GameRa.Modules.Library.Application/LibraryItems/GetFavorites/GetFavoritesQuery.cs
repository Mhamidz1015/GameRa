using GameRa.Common.Application.Messaging;
using GameRa.Modules.Library.Application.LibraryItems.GetUserLibrary;

namespace GameRa.Modules.Library.Application.LibraryItems.GetFavorites;

public sealed record GetFavoritesQuery(Guid UserId)
    : IQuery<IReadOnlyCollection<LibraryItemResponse>>;