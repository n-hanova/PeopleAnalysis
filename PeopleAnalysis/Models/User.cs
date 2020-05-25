using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace PeopleAnalysis.Models
{
    public class User : IdentityUser
    {
        public string Language { get; set; }
    }
}
