namespace Arcadia.Ask.Auth
{
    using System.Threading;
    using System.Threading.Tasks;

    using Arcadia.Ask.Auth.Roles;
    using Arcadia.Ask.Models.Entities;
    using Arcadia.Ask.Storage;
    using Arcadia.Ask.Storage.Users;

    using Microsoft.AspNetCore.Identity;

    public class SignInService : ISignInService
    {
        private readonly DatabaseContext dbCtx;
        private readonly IPasswordHasher<UserEntity> passwordHasher;
        private readonly IUserRepository userRepository;

        public SignInService(DatabaseContext dbCtx, IPasswordHasher<UserEntity> passwordHasher, IUserRepository userRepository)
        {
            this.dbCtx = dbCtx;
            this.passwordHasher = passwordHasher;
            this.userRepository = userRepository;
        }

        public async Task<bool> IsModeratorWithCredentialsExists(string login, string password, CancellationToken? token = null)
        {
            var moderator = await this.userRepository.FindUserByLoginAndRole(login, RoleNames.Moderator, token);
            if (moderator == null)
            {
                return false;
            }

            return this.passwordHasher.VerifyHashedPassword(moderator, moderator.Hash, password) == PasswordVerificationResult.Success;
        }
    }
}