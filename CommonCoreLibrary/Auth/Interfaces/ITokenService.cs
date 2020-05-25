using AuthAPI.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CommonCoreLibrary.Auth.Interfaces
{
    public interface IBaseTokenService
    {
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token, bool lifetime = true);
        string GenerateFullToken(string token);
        Task SignInAsync(IAuthResult loginResult);
    }

    public class ClientTokenService : IBaseTokenService
    {
        protected readonly AuthSettings authSettings;
        protected readonly IHttpContextAccessor httpContextAccessor;

        public ClientTokenService(AuthSettings authSettings, IHttpContextAccessor httpContextAccessor)
        {
            this.authSettings = authSettings;
            this.httpContextAccessor = httpContextAccessor;
        }

        public virtual string GenerateFullToken(string token) => $"Bearer {token}";

        public virtual ClaimsPrincipal GetPrincipalFromExpiredToken(string token, bool lifetime = true)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = lifetime,
                ValidAudience = authSettings.Audience,
                ValidIssuer = authSettings.Issuer,
                IssuerSigningKey = authSettings.SecurityKey
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }

        public virtual async Task SignInAsync(IAuthResult loginResult)
        {
            if (loginResult == null)
                return;
            var principal = GetPrincipalFromExpiredToken(loginResult.AccessToken);
            principal.Identities.First().AddClaim(new Claim("Token", loginResult.AccessToken));
            principal.Identities.First().AddClaim(new Claim("Refresh", loginResult.RefreshToken));
            await httpContextAccessor.HttpContext.SignOutAsync();
            await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties());
            /*for next request in this request*/
            httpContextAccessor.HttpContext.User = principal;
        }
    }
}
