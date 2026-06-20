using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Games.Presentation.Games;

public static class GameEndpoints
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        AddGame.MapEndpoint(app);
        GetGames.MapEndpoint(app);
        GetGame.MapEndpoint(app);
        DelistGame.MapEndpoint(app);
        ReleaseGame.MapEndpoint(app);
        SearchGames.MapEndpoint(app);
    }
}
