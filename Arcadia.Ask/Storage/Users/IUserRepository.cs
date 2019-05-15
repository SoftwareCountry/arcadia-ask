namespace Arcadia.Ask.Storage.Users
{
    using System.Threading;
    using System.Threading.Tasks;

    using Arcadia.Ask.Models.Entities;

    public interface IUserRepository
    {
        Task<UserEntity> FindUserByLoginAndRole(string login, string role, CancellationToken? token = null);
    }
}
