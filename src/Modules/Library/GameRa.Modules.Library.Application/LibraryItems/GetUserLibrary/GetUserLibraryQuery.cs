using GameRa.Common.Application.Messaging;
using GameRa.Modules.Library.Application.Abstractions;

namespace GameRa.Modules.Library.Application.LibraryItems.GetUserLibrary;

public sealed record GetUserLibraryQuery(
    Guid UserId,
    LibraryFilter Filter = LibraryFilter.Active)
    : IQuery<IReadOnlyCollection<LibraryItemResponse>>;