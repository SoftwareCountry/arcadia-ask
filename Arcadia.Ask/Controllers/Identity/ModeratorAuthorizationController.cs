namespace Arcadia.Ask.Controllers.Identity
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Arcadia.Ask.Auth;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/auth/moderator")]
    [ApiController]
    public class ModeratorAuthorizationController : ControllerBase
    {
        [Route("sign-in")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AuthorizeModerator()
        {
            var roleName = Roles.Moderator;

            if (this.User.IsInRole(roleName))
            {
                return this.Ok();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, this.User.Identity.Name),
                new Claim(ClaimTypes.Role, roleName)
            };
            var identity = new ClaimsIdentity(claims, roleName);
            var principal = new ClaimsPrincipal(identity);

            await this.HttpContext.SignInAsync(principal);

            return this.Ok();
        }
    }
}