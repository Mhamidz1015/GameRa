using System.Text.Json.Serialization;

namespace GameRa.Modules.Users.Infrastructure.Identity;

internal sealed record UserRepresentation(
    [property: JsonPropertyName("username")] string Username,
    [property: JsonPropertyName("email")] string Email,
    [property: JsonPropertyName("emailVerified")] bool EmailVerified,
    [property: JsonPropertyName("enabled")] bool Enabled,
    [property: JsonPropertyName("credentials")] CredentialRepresentation[] Credentials,
    [property: JsonPropertyName("requiredActions")] string[] RequiredActions);