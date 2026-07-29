using GameRa.Common.Application.Data;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Games.Application.Abstractions.Data;
using GameRa.Modules.Games.Domain.Categories;
using GameRa.Modules.Games.Domain.Games;
using MediatR;

namespace GameRa.Modules.Games.Application.Games.AddGame;

internal sealed class AddGameCommandHandler(IGameRepository GameRepository,
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<AddGameCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddGameCommand request, CancellationToken cancellationToken)
    {
        Category? category = await categoryRepository.GetAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            return Result.Failure<Guid>(CategoryErrors.NotFound(request.CategoryId));
        }

        Result<Game> result = Game.Create(
            category.Id,
            request.Title,
            request.Description,
            request.Developer,
            request.ReleaseDate,
            request.BasePrice,
            request.CoverImageUrl);

        if (result.IsFailure)
            return Result.Failure<Guid>(result.Error);

        GameRepository.Insert(result.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return result.Value.Id;
    }
}
