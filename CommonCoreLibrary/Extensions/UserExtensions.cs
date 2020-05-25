using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace CommonCoreLibrary.Extensions
{
    public static class UserExtensions
    {
        public static string Code(this ClaimsPrincipal claimsPrincipal) => claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "Code")?.Value;
        public static string UICode(this ClaimsPrincipal claimsPrincipal) => claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "UICode")?.Value;
        public static string Token(this ClaimsPrincipal claimsPrincipal) => claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "Token")?.Value;
        public static string Refresh(this ClaimsPrincipal claimsPrincipal) => claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "Refresh")?.Value;
        public static string Id(this ClaimsPrincipal claimsPrincipal) => claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
    }
}
