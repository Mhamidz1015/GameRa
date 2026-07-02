namespace GameRa.Modules.Users.Application.Users.GetUserById;

public sealed record UserResponse(Guid Id, string Email, string Username, DateTime CreatedOnUtc);
