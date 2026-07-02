using Microsoft.AspNetCore.Routing;

namespace GameRa.Common.Presentation.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
