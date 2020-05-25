using Microsoft.AspNetCore.Identity;
using PeopleAnalysis.Extensions;
using PeopleAnalysis.Models;

namespace PeopleAnalysis.Services
{
    public class AuthContextInitializer
    {
        public static void RolesInit(RoleManager<IdentityRole> roleManager)
        {
            string[] names = new[] { "Admin", "User" };
            foreach (var name in names)
            {
                if (!roleManager.RoleExistsAsync(name).Result())
                    roleManager.CreateAsync(new IdentityRole { Name = name }).ResultEmpty();
            }
        }

        public static void UsersInits(UserManager<User> userManager)
        {
            (string, string, string)[] users = new[]
            {
                ("Admin@yandex.ru", "Admin123!", "Admin"),
                ("User@yandex.ru", "User123!", "User")
            };

            foreach (var user in users)
            {
                if (userManager.FindByEmailAsync(user.Item1).Result() != null)
                    continue;
                var newUser = new User { Email = user.Item1, UserName = user.Item1 };
                if (userManager.CreateAsync(newUser, user.Item2).Result().Succeeded)
                    userManager.AddToRoleAsync(newUser, user.Item3).ResultEmpty();
            }
        }
    }
}
