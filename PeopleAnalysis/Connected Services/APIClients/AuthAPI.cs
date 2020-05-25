using CommonCoreLibrary.APIClient;
using CommonCoreLibrary.Auth.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading.Tasks;

namespace PeopleAnalysis.AuthAPI
{
    public partial class AuthAPIClient : BaseAPIClient
    {
        public AuthAPIClient(string baseUrl, HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IBaseTokenService tokenService) : base(httpContextAccessor, tokenService)
        {
            this._baseUrl = baseUrl;
            this._httpClient = httpClient;
            this.AuthAPIClient = this;
            _settings = new System.Lazy<Newtonsoft.Json.JsonSerializerSettings>(CreateSerializerSettings);
        }

        public Task AuthLoginGetAsync()
        {
            return ApiAuthLoginGetAsync();
        }

        public async Task<IAuthResult> AuthRefreshtokenAsync(string v1, string v2)
        {
            return await ApiAuthRefreshtokenAsync(v1, v2);
        }

        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url)
        {
            base.PrepareRequest(client, request, url);
        }
    }

    public partial class ApiException: IApiException
    {

    }
}
