namespace Arcadia.Ask.Auth
{
    using System;
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class GuidIdentificationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public GuidIdentificationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
        ) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            const string authorizationHeaderName = "Authorization";
            const string schemeName = "guid";

            if (!this.Request.Headers.ContainsKey(authorizationHeaderName))
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing authorization header"));
            }

            var authHeaderValue = AuthenticationHeaderValue.Parse(this.Request.Headers[authorizationHeaderName]);

            if (authHeaderValue.Scheme != schemeName)
            {
                return Task.FromResult(AuthenticateResult.Fail("Authentication type should be guid"));
            }

            var guid = authHeaderValue.Parameter;

            if (String.IsNullOrEmpty(guid))
            {
                return Task.FromResult(AuthenticateResult.Fail("no Guid was provided"));
            }

            try
            {
                Guid.Parse(guid);
            }
            catch (FormatException)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid guid format"));
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, guid),
            };

            var identity = new ClaimsIdentity(claims, schemeName);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, schemeName);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}