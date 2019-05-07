namespace Arcadia.Ask.Auth
{
    using System.Threading.Tasks;

    public interface ISignInService
    {
        Task<bool> IsModeratorWithCredentialsExists(string login, string password);

        Task<User> GetModeratorByCredentials(string login, string password);
    }
}