namespace Arcadia.Ask.Auth.Permissions
{
    public interface IPermissionsByRoleLoader
    {
        IPermissions GetByRole(string role);
    }
}