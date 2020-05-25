using AuthAPI.Models.Controller;
using AuthAPI.Models.Database;
using AuthAPI.Settings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AuthAPI.Services
{
    public interface IAuthService
    {
        Task<LoginResult> LoginAsync(LoginModel loginModel);
        Task<LoginResult> RegisterAsync(RegisterModel registerModel);
        Task<LoginResult> RestorePassword(PasswordRestore passwordRestore, string token);
        Task<LoginResult> RefreshToken(string refreshToken, string token);
    }

    public class AuthService : IAuthService
    {
        private readonly IAuthDataProvider authDataProvider;
        private readonly ITokenService tokenService;

        public AuthService(IAuthDataProvider authDataProvider, ITokenService tokenService)
        {
            this.authDataProvider = authDataProvider;
            this.tokenService = tokenService;
        }

        public async Task<LoginResult> LoginAsync(LoginModel loginModel)
        {
            var exists = authDataProvider.Users.FirstOrDefault(x => x.Email == loginModel.Login && x.PasswordHash == CryptService.CreateHash(loginModel.Password));
            if (exists == null)
                return null;
            var res = await GenerateTokenAndResult(exists);
            await authDataProvider.SaveChangesAsync();
            return res;
        }

        public async Task<LoginResult> RefreshToken(string refreshToken, string token)
        {
            var tokenInfo = tokenService.GetPrincipalFromExpiredToken(token, false);
            var user = authDataProvider.Users.FirstOrDefault(x => x.Email == tokenInfo.Identity.Name);
            if (user == null || user.RefreshToken != refreshToken)
                return null;

            var res = await GenerateTokenAndResult(user);
            await authDataProvider.SaveChangesAsync();
            return res;
        }

        public async Task<LoginResult> RegisterAsync(RegisterModel registerModel)
        {
            var exists = authDataProvider.Users.FirstOrDefault(x => x.Email == registerModel.Email);
            if (exists != null)
                return null;

            var newUser = new User
            {
                Email = registerModel.Email,
                Nickname = registerModel.Nickname,
                PasswordHash = CryptService.CreateHash(registerModel.Password),
                Role = authDataProvider.Roles.FirstOrDefault(x => x.Name == "User")
            };

            authDataProvider.Languages.Load();

            authDataProvider.Add(newUser);
            await authDataProvider.SaveChangesAsync();
            var res = await GenerateTokenAndResult(newUser);
            return res;
        }

        public async Task<LoginResult> RestorePassword(PasswordRestore passwordRestore, string token)
        {
            var tokenInfo = tokenService.GetPrincipalFromExpiredToken(token);
            var exists = authDataProvider.Users.FirstOrDefault(x => x.Id == passwordRestore.UserId);
            if (exists == null || exists.Email != tokenInfo.Identity.Name || exists.PasswordHash != CryptService.CreateHash(passwordRestore.PrevPassword))
                return null;

            exists.PasswordHash = CryptService.CreateHash(passwordRestore.Password);

            var res = await GenerateTokenAndResult(exists);
            await authDataProvider.SaveChangesAsync();
            return res;
        }

        private async Task<LoginResult> GenerateTokenAndResult(User user)
        {
            var refreshToken = Guid.NewGuid().ToString();
            user.RefreshToken = refreshToken;
            await authDataProvider.SaveChangesAsync();

            var claimsToken = tokenService.GenerateToken(user);

            return new LoginResult
            {
                AccessToken = claimsToken,
                RefreshToken = refreshToken
            };
        }
    }
}
