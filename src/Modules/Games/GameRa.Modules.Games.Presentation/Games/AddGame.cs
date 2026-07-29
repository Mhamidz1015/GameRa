using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Modules.Games.Application.Games.AddGame;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Games.Presentation.Games;

internal sealed class AddGame : IEndpoint
{
    public  void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("Games", async (Request request, ISender sender) =>
        {
            var command = new AddGameCommand(
                request.CategoryId,
                request.Title,
                request.Description,
                request.Developer,
                request.ReleaseDate,
                request.BasePrice,
                request.CoverImageUrl);

            Result<Guid> result = await sender.Send(command);
            if (result.IsFailure)
                return Results.BadRequest(result.Error);

            Guid gameId = result.Value;

            return Results.Ok(gameId);
        })
        .WithTags(Tags.Games);
    }

    internal sealed class Request
    {
       public string Title { get; private set; }

        public string Description { get; private set; }

        public  string Developer { get; private set; }

        public DateTime ReleaseDate { get; private set; }

        public decimal BasePrice { get; private set; }

        public string CoverImageUrl { get; private set; }
        public Guid CategoryId { get; private set; }
    }
}
