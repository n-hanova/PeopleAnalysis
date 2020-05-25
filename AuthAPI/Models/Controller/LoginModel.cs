using System.ComponentModel.DataAnnotations;

namespace AuthAPI.Models.Controller
{
    public class LoginModel
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
