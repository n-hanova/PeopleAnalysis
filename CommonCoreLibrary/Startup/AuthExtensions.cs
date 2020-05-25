using AuthAPI.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommonCoreLibrary.Startup
{
    public static class AuthExtensions
    {
        public static void AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var authSettings = configuration.GetSection("AuthSettings").Get<AuthSettings>();

            services.AddSingleton(authSettings);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidAudience = authSettings.Audience,
                    ValidIssuer = authSettings.Issuer,
                    IssuerSigningKey = authSettings.SecurityKey
                };
            });
        }
    }
}
