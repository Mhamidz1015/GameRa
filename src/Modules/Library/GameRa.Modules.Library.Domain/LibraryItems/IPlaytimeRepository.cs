using System;
using System.Collections.Generic;
using System.Text;

namespace GameRa.Modules.Library.Domain.LibraryItems
{
    public interface IPlaytimeRepository
    {
        Task<PlaytimeRecord?> GetAsync(Guid UserId, Guid GameId, CancellationToken cancellationToken = default);

        void Insert(PlaytimeRecord playtimeRecord);
    }
}
