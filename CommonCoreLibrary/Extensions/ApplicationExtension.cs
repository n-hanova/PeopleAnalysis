using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace PeopleAnalysis.Extensions
{
    public static class ApplicationExtension
    {
        public static IActionResult Error<T>(this Controller controller, T obj, string error)
        {
            controller.TempData["Error"] = error;
            return controller.View(obj);
        }

        public static string UserId(this ClaimsPrincipal claimsPrincipal) => claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType)?.Value;
        public static string UserRole(this ClaimsPrincipal claimsPrincipal) => claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultRoleClaimType)?.Value;
        public static bool IsAdmin(this ClaimsPrincipal claimsPrincipal) => claimsPrincipal.UserRole() == "Admin";
    }
}
