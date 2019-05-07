namespace Arcadia.Ask.Auth
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    using Arcadia.Ask.Auth.Roles;
    using Arcadia.Ask.Storage;

    using Microsoft.EntityFrameworkCore;

    public class SignInService : ISignInService
    {
        private readonly DatabaseContext dbCtx;

        public SignInService(DatabaseContext dbCtx)
        {
            this.dbCtx = dbCtx;
        }

        private static string ComputeHashFromString(string source)
        {
            using (var sha1 = SHA1.Create())
            {
                var buffer = Encoding.ASCII.GetBytes(source);

                var hash = sha1.ComputeHash(buffer);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        public async Task<bool> IsModeratorWithCredentialsExists(string login, string password)
        {
            var hashedPassword = ComputeHashFromString(password);

            return await this.dbCtx.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Where(u =>
                    u.UserRoles.Any(ur => ur.Role.Name == RoleNames.Moderator))
                .AnyAsync(m => m.Login == login && m.Hash == hashedPassword);
        }
    }
}