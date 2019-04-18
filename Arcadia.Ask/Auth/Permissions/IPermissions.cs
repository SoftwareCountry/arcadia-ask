namespace Arcadia.Ask.Auth.Permissions
{
    public interface IPermissions
    {
        bool CanVote { get; }

        bool CanCreateQuestion { get; }

        bool CanApproveQuestion { get; }

        bool CanDeleteQuestion { get; }

        bool CanHideQuestion { get; }

        bool CanDisplayQuestion { get; }
    }
}