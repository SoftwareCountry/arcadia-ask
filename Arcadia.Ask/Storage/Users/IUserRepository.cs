namespace Arcadia.Ask.Storage.Users
{
    using System.Threading;
    using System.Threading.Tasks;

    using Arcadia.Ask.Models.Entities;

    public interface IUserRepository
    {
        Task<UserEntity> FindUserByLogin(string login, CancellationToken token = default(CancellationToken));
    }
}
