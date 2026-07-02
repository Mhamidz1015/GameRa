using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Users.Domain.Users;

public sealed class User : Entity
{
    private readonly List<Role> _roles = [];
    private User()
    {
    }

    public Guid Id { get; private set; }

    public string Email { get; private set; }

    public string Username { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public string IdentityId { get; private set; }

    public IReadOnlyCollection<Role> Roles => _roles.ToList();

    public static User Create(string email, string username, DateTime createdOnUtc,string identityId )
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            Username = username,
            CreatedOnUtc = createdOnUtc,
            IdentityId = identityId
            
        };

        user._roles.Add(Role.Member);

        user.Raise(new UserRegisteredDomainEvent(user.Id));

        return user;
    }

    public void Update(string username)
    {
        if (Username == username)
        {
            return;
        }

        Username = username;

        Raise(new UserProfileUpdatedDomainEvent(Id, Username));
    }
}
