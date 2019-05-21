namespace Arcadia.Ask.Auth
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Arcadia.Ask.Auth.Roles;
    using Arcadia.Ask.Storage.Users;

    using Microsoft.AspNetCore.Identity;

    public class SignInService : ISignInService
    {
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly IUserRepository userRepository;

        public SignInService(IPasswordHasher<User> passwordHasher, IUserRepository userRepository)
        {
            this.passwordHasher = passwordHasher;
            this.userRepository = userRepository;
        }

        public async Task<User> GetUserByCredentials(string login, string password, CancellationToken token)
        {
            var foundUser = await this.userRepository.FindUserByLogin(login, token);

            if (foundUser == null)
            {
                throw new UserWasNotFoundException();
            }

            var user = new User(login, foundUser.UserRoles.Select(r => r.Role.Name));

            return this.passwordHasher.VerifyHashedPassword(user, foundUser.Hash, password) == PasswordVerificationResult.Success
                ? user
                : throw new UserWasNotFoundException();
        }
    }
}