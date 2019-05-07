namespace Arcadia.Ask.Auth
{
    using System;

    public class UserWasNotFoundException : Exception
    {
        public UserWasNotFoundException() : base("user was not found")
        {
        }
    }
}