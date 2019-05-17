namespace Arcadia.Ask.Auth
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Arcadia.Ask.Auth.Roles;
    using Arcadia.Ask.Models.Entities;
    using Arcadia.Ask.Storage.Users;

    using Microsoft.AspNetCore.Identity;

    public class SignInService : ISignInService
    {
        private readonly IPasswordHasher<UserEntity> passwordHasher;
        private readonly IUserRepository userRepository;

        public SignInService(IPasswordHasher<UserEntity> passwordHasher, IUserRepository userRepository)
        {
            this.passwordHasher = passwordHasher;
            this.userRepository = userRepository;
        }

        public async Task<bool> IsModeratorWithCredentialsExists(string login, string password, CancellationToken token)
        {
            var moderator = await this.userRepository.FindUserByLogin(login, token);

            if (moderator == null)
            {
                return false;
            }

            if (moderator.UserRoles.All(r => r.Role.Name != RoleNames.Moderator))
            {
                return false;
            }

            return this.passwordHasher.VerifyHashedPassword(moderator, moderator.Hash, password) == PasswordVerificationResult.Success;
        }
    }
}