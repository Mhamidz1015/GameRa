using GameRa.Common.Application.Authorization;
using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Users.Application.Users.GetUserPermissions;

public sealed record GetUserPermissionsQuery(string IdentityId) : IQuery<PermissionsResponse>;
