using AuthAPI.Models.Database;
using System.Linq;

namespace AuthAPI.Services
{
    public interface IUserService
    {
        User GetUser(string token);
    }

    public class UserService : IUserService
    {
        private readonly IAuthDataProvider authDataProvider;
        private readonly ITokenService tokenService;

        public UserService(IAuthDataProvider authDataProvider, ITokenService tokenService)
        {
            this.authDataProvider = authDataProvider;
            this.tokenService = tokenService;
        }

        public User GetUser(string token)
        {
            var tokenInfo = tokenService.GetPrincipalFromExpiredToken(token);
            return authDataProvider.Users.FirstOrDefault(x => x.Email == tokenInfo.Identity.Name);
        }
    }
}
