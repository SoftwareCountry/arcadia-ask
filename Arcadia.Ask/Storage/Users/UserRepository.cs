﻿namespace Arcadia.Ask.Storage.Users
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Arcadia.Ask.Auth.Roles;
    using Arcadia.Ask.Models.Entities;

    using Microsoft.EntityFrameworkCore;

    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext dbCtx;

        public UserRepository(DatabaseContext dbCtx)
        {
            this.dbCtx = dbCtx;
        }

        public async Task<UserEntity> FindUserByLoginAndRole(string login, string role, CancellationToken? token = null)
        {
            var found = await this.dbCtx.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Where(u =>
                    u.UserRoles.Any(ur => ur.Role.Name == RoleNames.Moderator) && u.Login == login).FirstOrDefaultAsync(token ?? CancellationToken.None);

            if (found == null)
            {
                return null;
            }

            this.dbCtx.Entry(found).State = EntityState.Detached;
            return found;
        }
    }
}