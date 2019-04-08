namespace Arcadia.Ask.Auth.Permissions
{
    public interface IPermissionsByRoleCreator
    {
        IPermissions Create(string role);
    }
}