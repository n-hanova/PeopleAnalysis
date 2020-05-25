using CommonCoreLibrary.Auth.Interfaces;
using CommonCoreLibrary.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace CommonCoreLibrary.APIClient
{
    public class BaseAPIClient
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IBaseTokenService tokenService;
        public IAuthBaseAPIClient AuthAPIClient { get; protected set; }

        private readonly string[] authEndPoints = new[] {
            "/api/Auth/refreshToken",
            "/api/Auth/login"
        };

        protected ClaimsPrincipal User => httpContextAccessor?.HttpContext?.User;

        public BaseAPIClient()
        {
            this.httpContextAccessor = null;
            this.tokenService = null;
        }

        public BaseAPIClient(IHttpContextAccessor httpContextAccessor, IBaseTokenService tokenService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.tokenService = tokenService;
        }

        protected void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!url.IsContains(authEndPoints))
                {
                    try
                    {
                        AuthAPIClient.AuthLoginGetAsync().GetAwaiter().GetResult();
                    }
                    catch (Exception ex)
                    {
                        if (((IApiException)ex)?.StatusCode != 401)
                            throw;

                        var result = AuthAPIClient.AuthRefreshtokenAsync(User.Refresh(), User.Token()).GetAwaiter().GetResult();
                        tokenService.SignInAsync(result);
                    }
                }

                client.DefaultRequestHeaders.Remove("Authorization");
                request.Headers.Remove("Authorization");
                request.Headers.Add("Authorization", tokenService.GenerateFullToken(httpContextAccessor.HttpContext.User.Token()));
                client.DefaultRequestHeaders.Add("Authorization", tokenService.GenerateFullToken(httpContextAccessor.HttpContext.User.Token()));
            }
        }
    }
}
