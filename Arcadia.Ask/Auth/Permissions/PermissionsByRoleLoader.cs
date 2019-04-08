namespace Arcadia.Ask.Auth.Permissions
{
    using System;

    using Arcadia.Ask.Auth.Roles;

    public class PermissionsByRoleLoader : IPermissionsByRoleLoader
    {
        public IPermissions GetByRole(string role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            switch (role)
            {
                case RoleNames.Moderator:
                    return new ModeratorPermissions();
                case RoleNames.User:
                    return new UserPermissions();
                default:
                    throw new ArgumentException($"Role {role} is unknown");
            }
        }
    }
}