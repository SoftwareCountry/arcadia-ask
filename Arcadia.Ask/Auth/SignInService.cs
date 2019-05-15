namespace Arcadia.Ask.Auth
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Arcadia.Ask.Models.Entities;
    using Arcadia.Ask.Storage;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class SignInService : ISignInService
    {
        private readonly DatabaseContext dbCtx;
        private readonly IPasswordHasher<ModeratorEntity> passwordHasher;

        public SignInService(DatabaseContext dbCtx, IPasswordHasher<ModeratorEntity> passwordHasher)
        {
            this.dbCtx = dbCtx;
            this.passwordHasher = passwordHasher;
        }

        public async Task<bool> IsModeratorWithCredentialsExists(string login, string password, CancellationToken token = default(CancellationToken))
        {
            var moderator = await this.dbCtx.Moderators
                .Where(m => m.Login == login).FirstOrDefaultAsync(token);

            if (moderator == null)
            {
                return false;
            }

            return this.passwordHasher.VerifyHashedPassword(moderator, moderator.Hash, password) == PasswordVerificationResult.Success;
        }
    }
}