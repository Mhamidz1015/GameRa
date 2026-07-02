namespace GameRa.Modules.Users.Infrastructure.Identity;

internal sealed record UserRepresentation(
    string Username,
    string Email,
    bool EmailVerified,
    bool Enabled,
    CredentialRepresentation[] Credentials);
