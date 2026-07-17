using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Games.Domain.Games;

public static class GameErrors
{
    public static Error NotFound(Guid gameId) =>
        Error.NotFound("Games.NotFound", $"The game with the identifier {gameId} was not found");

    public static readonly Error Released = Error.Problem(
        "Games.Released",
        "The game is not in ComingSoon status");

    public static readonly Error AlreadyDelisted = Error.Problem(
        "Games.AlreadyDelisted",
        "The game was already Delisted");

    public static readonly Error GenreAlreadyAssigned = Error.Problem(
        "Games.AlreadyAssigned",
        "The genre was already AlreadyAssigned To The Game");

    public static readonly Error TitleIsEmpty = Error.Problem(
        "Games.TitleIsEmpty",
        "The title Is Empty In The Game.title");

    public static readonly Error PriceCannotBeNegative = Error.Problem(
        "Games.PriceCannotbeNegative",
        "The price CannotBeNegative In The GamePrice");

    public static readonly Error DescriptionIsEmpty = Error.Problem(
        "Games.DescriptionIsEmpty",
        "The description Is Empty In The Game.description");
}
