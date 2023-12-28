using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities
{
    public class User
    {
        public UserId Id { get; }
        public Email Email { get; }
        public Username Username { get; }
        public Password Password { get; }
        public Fullname Fullname { get; }
        public Role Role { get; }
        public DateTime CreatedAt { get; }

        public User(UserId id, Email email, Username username, Password password, Fullname fullname, Role role, DateTime createdAt)
        {
            Id = id;
            Email = email;
            Username = username;
            Password = password;
            Fullname = fullname;
            Role = role;
            CreatedAt = createdAt;
        }

        private User()
        {
        }
    }
}
