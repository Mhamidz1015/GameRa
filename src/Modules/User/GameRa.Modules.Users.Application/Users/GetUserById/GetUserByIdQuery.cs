using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Users.Application.Users.GetUserById;

public sealed record GetUserByIdQuery(Guid UserId) : IQuery<UserResponse>;
