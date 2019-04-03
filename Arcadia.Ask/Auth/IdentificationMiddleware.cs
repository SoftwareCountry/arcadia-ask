namespace Arcadia.Ask.Auth
{
    using System;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;

    public static class IdentificationMiddleware
    {
        public const string CookieName = "User";

        public static IApplicationBuilder UseGuidIdentification(this IApplicationBuilder app)
        {
            return app.Use(async (context, next) =>
            {
                if (string.IsNullOrEmpty(context.Request.Cookies[CookieName]))
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.Role, "User")
                    };
                    var identity = new ClaimsIdentity(claims, CookieName);
                    var principal = new ClaimsPrincipal(identity);
                    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                }

                await next.Invoke();
            });
        }
    }
}