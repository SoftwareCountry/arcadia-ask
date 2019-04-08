namespace Arcadia.Ask.Auth.Permissions
{
    using System;

    using Arcadia.Ask.Auth.Roles;

    public class PermissionsByRoleCreator
    {
        public IPermissions Permissions { get; }

        public PermissionsByRoleCreator(string role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            switch (role)
            {
                case RoleNames.Moderator:
                    this.Permissions = new ModeratorPermissions();
                    break;
                case RoleNames.User:
                    this.Permissions = new UserPermissions();
                    break;
                default:
                    throw new ArgumentException($"Role {role} is unknown");
            }
        }
    }
}