using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthAPI.Settings
{
    public class AuthSettings
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string Key { get; set; }
        public int AccessExpiration { get; set; }
        public SecurityKey SecurityKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    }
}
