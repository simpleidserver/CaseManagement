using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace CaseManagement.HumanTasks.Acceptance.Tests.Middlewares
{
    public class CustomAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IScenarioContextProvider _scenarioContextProvider;

        public CustomAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IScenarioContextProvider scenarioContextProvider) : base(options, logger, encoder, clock)
        {
            _scenarioContextProvider = scenarioContextProvider;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var scenarioContext = _scenarioContextProvider.GetContext();
            var claims = new List<Claim>();
            if (scenarioContext.ContainsKey("claims"))
            {
                claims = scenarioContext.Get<List<Claim>>("claims");
            }
            else
            {
                claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "Thierry Habart"),
                    new Claim(ClaimTypes.NameIdentifier, "thabart")
                };
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var authenticationTicket = new AuthenticationTicket(
                                             claimsPrincipal,
                                             new AuthenticationProperties(),
                                             CookieAuthenticationDefaults.AuthenticationScheme);
            return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
        }
    }
}
