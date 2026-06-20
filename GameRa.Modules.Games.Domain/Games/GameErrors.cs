using GameRa.Modules.Games.Domain.Abstractions;

namespace GameRa.Modules.Games.Domain.Games;

public static class GameErrors
{
    public static Error NotFound(Guid gameId) =>
        Error.NotFound("Games.NotFound", $"The game with the identifier {gameId} was not found");

    public static readonly Error Released = Error.Problem("Games.Released", "The event is not in ComingSoon status");

    public static readonly Error AlreadyDelisted = Error.Problem(
        "Games.AlreadyDelisted",
        "The game was already Delisted");

    public static readonly Error GenreAlreadyAssigned = Error.Problem(
        "Games.AlreadyAssigned",
        "The genre was already AlreadyAssigned To The Game");
}
