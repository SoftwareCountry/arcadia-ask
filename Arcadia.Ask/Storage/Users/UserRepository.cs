namespace Arcadia.Ask.Storage.Users
{
    using System.Threading;
    using System.Threading.Tasks;

    using Arcadia.Ask.Models.Entities;

    using Microsoft.EntityFrameworkCore;

    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext dbCtx;

        public UserRepository(DatabaseContext dbCtx)
        {
            this.dbCtx = dbCtx;
        }

        public async Task<UserEntity> FindUserByLogin(string login, CancellationToken token)
        {
            return await this.dbCtx.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == login, token);
        }
    }
}