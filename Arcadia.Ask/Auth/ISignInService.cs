namespace Arcadia.Ask.Auth
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface ISignInService
    {
        Task<bool> IsModeratorWithCredentialsExists(string login, string password, CancellationToken token = default(CancellationToken));
    }
}