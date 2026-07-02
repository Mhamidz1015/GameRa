using GameRa.Common.Domain.Abstractions;

namespace GameRa.Common.Application.Authorization;

public interface IPermissionService
{
    Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId);
}
