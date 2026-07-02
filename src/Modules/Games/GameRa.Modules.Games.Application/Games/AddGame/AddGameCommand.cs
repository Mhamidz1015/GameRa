using MediatR;

namespace GameRa.Modules.Games.Application.Games.AddGame;

public sealed record AddGameCommand(
    
    string Title,
    string Description,
    string Developer,
    DateTime ReleaseDate,
    decimal BasePrice,
    string CoverImageUrl) : IRequest<Guid>;
