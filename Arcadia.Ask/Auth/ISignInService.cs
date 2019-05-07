namespace Arcadia.Ask.Auth
{
    using System.Threading.Tasks;

    public interface ISignInService
    {
        Task<User> GetUserByCredentials(string login, string password);
    }
}