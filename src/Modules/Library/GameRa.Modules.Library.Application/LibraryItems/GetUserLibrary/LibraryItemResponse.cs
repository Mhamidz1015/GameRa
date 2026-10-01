using GameRa.Common.Application.Clock;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRa.Modules.Library.Application.LibraryItems.GetUserLibrary;
    public sealed record class LibraryItemResponse(
            Guid Id,
            Guid GameId,
            Guid UserId,
            string GameTitleSnapshot,
            bool IsArchived,
            bool IsFavorite);
       
