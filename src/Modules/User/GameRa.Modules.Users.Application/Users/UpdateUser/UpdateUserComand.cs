using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Users.Application.Users.UpdateUser;

public sealed record UpdateUserCommand(Guid UserId, string Username) : ICommand;
