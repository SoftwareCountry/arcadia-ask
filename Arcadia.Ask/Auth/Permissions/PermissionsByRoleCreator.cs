namespace Arcadia.Ask.Auth.Permissions
{
    using Arcadia.Ask.Auth.Roles;

    public class PermissionsByRoleCreator
    {
        public IPermissions Permissions { get; }

        public PermissionsByRoleCreator(string role)
        {
            if (role == Roles.Moderator)
            {
                this.Permissions = new ModeratorPermissions();
            }
            else if (role == Roles.User)
            {
                this.Permissions = new UserPermissions();
            }
            else
            {
                throw new UnknownRoleException(role);
            }
        }
    }
}