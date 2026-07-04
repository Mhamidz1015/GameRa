using GameRa.Common.Application.Exceptions;
using GameRa.Common.Infrastructure.Authentication;
using GameRa.Modules.Store.Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace GameRa.Modules.Store.Infrastructure.Authentication;

internal sealed class CustomerContext(IHttpContextAccessor httpContextAccessor) : ICustomerContext
{
    public Guid CustomerId => httpContextAccessor.HttpContext?.User.GetUserId() ??
                              throw new GameRaException("User identifier is unavailable");
}
