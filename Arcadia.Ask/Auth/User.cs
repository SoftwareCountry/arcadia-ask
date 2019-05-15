namespace Arcadia.Ask.Auth
{
    using System.Collections.Generic;

    public class User
    {
        public User(string login, IEnumerable<string> roles)
        {
            this.Login = login;
            this.Roles = roles;
        }

        public string Login { get; }

        public IEnumerable<string> Roles { get; }
    }
}