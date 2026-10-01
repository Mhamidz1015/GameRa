using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Library.Application.Abstractions.Data;
using GameRa.Modules.Library.Domain;
using GameRa.Modules.Library.Domain.LibraryItems;

namespace GameRa.Modules.Library.Application.Playtime.RecordPlaytime;

internal sealed class RecordPlaytimeCommandHandler(
    IPlaytimeRepository playtimeRepository,
    ILibraryItemRepository libraryItemRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RecordPlaytimeCommand>
{
    public async Task<Result> Handle(RecordPlaytimeCommand request, CancellationToken cancellationToken)
    {
        bool ownsGame = await libraryItemRepository.ExistsAsync(
            request.UserId, request.GameId, cancellationToken);

        if (!ownsGame)
            return Result.Failure(LibraryItemErrors.NotOwned(request.GameId));

        PlaytimeRecord? record = await playtimeRepository.GetAsync(
            request.UserId, request.GameId, cancellationToken);

        if (record is null)
        {
            record = PlaytimeRecord.Create(request.UserId, request.GameId);
            playtimeRepository.Insert(record);
        }

        record.AddPlaytime(request.Minutes);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}