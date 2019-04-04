namespace Arcadia.Ask.Controllers.Identity
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/auth/moderator")]
    [ApiController]
    public class ModeratorAuthorizationController : ControllerBase
    {
        [Route("sign-in")]
        [HttpPost]
        public async Task<IActionResult> AuthorizeModerator()
        {
            const string roleName = "Moderator";

            if (this.User.IsInRole(roleName))
            {
                return this.Ok();
            }

            var guid = this.User.Identity.IsAuthenticated ? 
                Guid.Parse(this.User.Identity.Name) : Guid.NewGuid();

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, guid.ToString()),
                new Claim(ClaimTypes.Role, roleName), 
            };
            var identity = new ClaimsIdentity(claims, roleName);
            var principal = new ClaimsPrincipal(identity);

            await this.HttpContext.SignInAsync(principal);

            return this.Ok();
        }
    }
}