namespace Arcadia.Ask.Auth
{
    using System.Threading.Tasks;

    public interface ISignInService
    {
        Task<bool> IsModeratorWithCredentialsExists(string login, string password);

        Task<User> GetUserByCredentials(string login, string password);
    }
}