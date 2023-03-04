﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using React.Data;
using System.Security.Claims;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;

namespace React.Handler
{
    public class AuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IReactRepo _repository;
        public AuthHandler(
            IReactRepo repository,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _repository = repository;
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                Response.Headers.Add("www-authenticate", "Basic");
                return AuthenticateResult.Fail("User not found");
            }
            else
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(":");
                var username = credentials[0];
                var password = credentials[1];
                
                if (_repository.CheckUser(username, password)) /* For normal user*/
                {
                    var claims = new[] { new Claim("normalUser", username),
                    new Claim("password", password)};

                    ClaimsIdentity identity = new ClaimsIdentity(claims, "Basic");
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    AuthenticationTicket ticket = new AuthenticationTicket(principal, Scheme.Name);

                    return AuthenticateResult.Success(ticket);
                }
                else
                {
                    Response.Headers.Add("www-authenticate", "Basic");
                    return AuthenticateResult.Fail("User not found");
                }
            }
        }
    }


}