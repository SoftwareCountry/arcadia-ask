namespace Arcadia.Ask.Auth
{
    using System;

    public class UserWasNotFoundException : Exception
    {
        public UserWasNotFoundException() : base("User was not found")
        {
        }
    }
}