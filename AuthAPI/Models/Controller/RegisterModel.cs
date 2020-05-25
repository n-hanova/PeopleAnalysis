using System.ComponentModel.DataAnnotations;

namespace AuthAPI.Models.Controller
{
    public class RegisterModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string Nickname { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
