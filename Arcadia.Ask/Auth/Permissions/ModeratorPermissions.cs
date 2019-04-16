namespace Arcadia.Ask.Auth.Permissions
{
    public class ModeratorPermissions : IPermissions
    {
        public bool CanVote { get; } = true;

        public bool CanCreateQuestion { get; } = true;

        public bool CanApproveQuestion { get; } = true;

        public bool CanDeleteQuestion { get; } = true;

        public bool CanHideQuestion { get; } = true;

        public bool CanDisplayQuestion { get; } = true;
    }
}