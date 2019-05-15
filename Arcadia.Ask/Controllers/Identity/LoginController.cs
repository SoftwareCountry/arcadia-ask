namespace Arcadia.Ask.Controllers.Identity
{
    using System;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;

    using Arcadia.Ask.Auth;
    using Arcadia.Ask.Auth.Roles;
    using Arcadia.Ask.Models.Requests;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/auth")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ISignInService signInService;

        public LoginController(ISignInService signInService)
        {
            this.signInService = signInService;
        }

        [Route("moderator/sign-in")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SignInAsModerator(ModeratorSignInRequestModel req, CancellationToken token)
        {
            const string roleName = RoleNames.Moderator;

            if (this.User.IsInRole(roleName))
            {
                return this.Ok();
            }

            var isCredentialsValid = await this.signInService.IsModeratorWithCredentialsExists(req.Login, req.Password, token);

            if (!isCredentialsValid)
            {
                return this.Unauthorized();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, req.Login), 
                new Claim(ClaimTypes.Name, this.User.Identity.Name),
                new Claim(ClaimTypes.Role, roleName)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await this.HttpContext.SignInAsync(principal);

            return this.Ok();
        }

        [Route("")]
        public async Task<ActionResult> SignIn()
        {
            var returnUrl = this.Request.Query[CookieAuthenticationDefaults.ReturnUrlParameter].ToString();
            var redirectUrl = string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl;
            
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, RoleNames.User)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return this.Redirect(redirectUrl);
        }
    }
}