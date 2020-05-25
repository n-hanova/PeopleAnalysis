using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CommonCoreLibrary.Auth.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PeopleAnalysis.AuthAPI;

namespace PeopleAnalysis.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly IAuthAPIClient authAPIClient;
        private readonly IBaseTokenService tokenService;

        public RegisterModel(IAuthAPIClient authAPIClient, IBaseTokenService tokenService)
        {
            this.authAPIClient = authAPIClient;
            this.tokenService = tokenService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await authAPIClient.ApiAuthRegisterAsync(new AuthAPI.RegisterModel { Email = Input.Email, Nickname = Input.Email, Password = Input.Password });
                    await tokenService.SignInAsync(result);
                    return LocalRedirect(returnUrl);
                }
                catch (ApiException)
                {
                    ModelState.AddModelError(string.Empty, "Invalid register attempt.");
                }
            }

            return Page();
        }
    }
}