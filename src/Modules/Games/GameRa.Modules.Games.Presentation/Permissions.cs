namespace GameRa.Modules.Games.Presentation;

internal static class Permissions
{
    // Categories
    internal const string GetCategories = "categories:read";
    internal const string CreateCategory = "categories:create";
    internal const string UpdateCategory = "categories:update";
    internal const string ArchiveCategory = "categories:archive";

    // Games
    internal const string GetGames = "games:read";
    internal const string AddGame = "games:add";
    internal const string ReleaseGame = "games:release";
    internal const string DelistGame = "games:delist";
}