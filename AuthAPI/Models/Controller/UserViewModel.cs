using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthAPI.Models.Controller
{
    public class UserViewModel
    {
        public string Id { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Login { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = string.Empty;
        [Required]
        public string Role { get; set; } = string.Empty;
    }
}
