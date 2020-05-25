using AnalyticAPI.AuthAPI;
using CommonCoreLibrary.APIClient;
using CommonCoreLibrary.Auth.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace AnalyticAPI.ApplicationAPI
{
    public partial class ApplicationAPIClient : BaseAPIClient
    {
        public ApplicationAPIClient(string baseUrl, HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IBaseTokenService tokenService, IAuthAPIClient authBaseAPI) : base(httpContextAccessor, tokenService)
        {
            this._baseUrl = baseUrl;
            this._httpClient = httpClient;
            this.AuthAPIClient = authBaseAPI;
            _settings = new System.Lazy<Newtonsoft.Json.JsonSerializerSettings>(CreateSerializerSettings);
            _settings.Value.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }

        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url)
        {
            base.PrepareRequest(client, request, url);
        }
    }

    public partial class ApiException : IApiException
    {

    }
}
