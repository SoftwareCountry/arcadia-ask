namespace Arcadia.Ask.Controllers.Identity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

            try
            {
                var moderator = await this.signInService.GetModeratorByCredentials(req.Login, req.Password, token);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, moderator.Login),
                    new Claim(ClaimTypes.Name, this.User.Identity.Name)
                };

                claims.AddRange(
                    moderator.Roles?.Select(r => new Claim(ClaimTypes.Role, r))
                    ?? new[]
                    {
                        new Claim(ClaimTypes.Role, roleName)
                    }
                );

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await this.HttpContext.SignInAsync(principal);

                return this.Ok();
            }
            catch (UserWasNotFoundException)
            {
                return this.Unauthorized();
            }
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