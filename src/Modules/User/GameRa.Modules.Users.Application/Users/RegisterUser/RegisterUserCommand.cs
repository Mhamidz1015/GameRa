using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Users.Application.Users.RegisterUser;

public sealed record RegisterUserCommand(string Email, string Password, string UserName, DateTime CreatedOnUtc)
    : ICommand<Guid>;
