using GameRa.Common.Application.Exceptions;
using GameRa.Common.Infrastructure.Authentication;
using GameRa.Modules.Library.Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace GameRa.Modules.Library.Infrastructure.Authentication;

internal sealed class LibraryItemContext(IHttpContextAccessor httpContextAccessor) : ILibraryItemContext
{
    public Guid LibraryItemId => httpContextAccessor.HttpContext?.User.GetUserId() ??
                              throw new GameRaException("User identifier is unavailable");
}
