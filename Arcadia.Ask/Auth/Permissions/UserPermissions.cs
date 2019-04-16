namespace Arcadia.Ask.Auth.Permissions
{
    public class UserPermissions : IPermissions
    {
        public bool CanVote { get; } = true;

        public bool CanCreateQuestion { get; } = true;

        public bool CanApproveQuestion { get; } = false;

        public bool CanDeleteQuestion { get; } = false;

        public bool CanHideQuestion { get; } = false;

        public bool CanDisplayQuestion { get; } = false;
    }
}