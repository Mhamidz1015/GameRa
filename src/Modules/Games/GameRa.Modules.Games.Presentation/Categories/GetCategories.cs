using GameRa.Common.Application.Caching;
using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Games.Application.Categories.GetCategories;
using GameRa.Modules.Games.Application.Categories.GetCategory;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Games.Presentation.Categories;

internal sealed class GetCategories : IEndpoint
{
    public  void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("categories", async (ISender sender, ICacheService cacheService) =>
        {
            IReadOnlyCollection<CategoryResponse> categoryResponses = 
                await cacheService.GetAsync<IReadOnlyCollection<CategoryResponse>>("categories");

            if (categoryResponses is not null) 
            {
                return Results.Ok(categoryResponses);
            }
                
            Result <IReadOnlyCollection<CategoryResponse>> result = await sender.Send(new GetCategoriesQuery());

            if(result.IsSuccess)
            {
                await cacheService.SetAsync("categories", result.Value);
            }

            return result.Match(Results.Ok, ApiResults.Problem);
        })
         .RequireAuthorization()
        .WithTags(Tags.Categories);
    }
}
