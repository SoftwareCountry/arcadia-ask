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
            const string guidParamName = "guid";

            if (!this.Request.Query.TryGetValue(guidParamName, out var guid))
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing guid query param"));
            }

            if (string.IsNullOrEmpty(guid))
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

            var identity = new ClaimsIdentity(claims, this.Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, this.Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}