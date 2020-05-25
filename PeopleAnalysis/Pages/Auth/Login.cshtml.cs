using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CommonCoreLibrary.Auth.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PeopleAnalysis.AuthAPI;

namespace PeopleAnalysis
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IAuthAPIClient authAPIClient;
        private readonly IBaseTokenService tokenService;

        public LoginModel(IAuthAPIClient authAPIClient, IBaseTokenService tokenService)
        {
            this.authAPIClient = authAPIClient;
            this.tokenService = tokenService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await authAPIClient.ApiAuthLoginPostAsync(new AuthAPI.LoginModel { Login = Input.Email, Password = Input.Password });
                    await tokenService.SignInAsync(result);
                    return LocalRedirect(returnUrl);
                }
                catch (ApiException)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }

            return Page();
        }
    }
}