namespace Arcadia.Ask.Auth.Roles
{
    using System;

    public class UnknownRoleException : Exception
    {
        public UnknownRoleException(string role) : base($"Role {role} is unknown") { }
    }
}