using CommonCoreLibrary.APIClient;
using CommonCoreLibrary.Auth.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PeopleAnalysis.AuthAPI;
using System.Net.Http;

namespace PeopleAnalysis.ApplicationAPI
{
    public partial class ApplicationAPIClient : BaseAPIClient
    {
        public ApplicationAPIClient(string baseUrl, HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IBaseTokenService tokenService, IAuthAPIClient authBaseAPI) : base(httpContextAccessor, tokenService)
        {
            this._baseUrl = baseUrl;
            this._httpClient = httpClient;
            this.AuthAPIClient = authBaseAPI;
            _settings = new System.Lazy<Newtonsoft.Json.JsonSerializerSettings>(() => new JsonSerializerSettings());
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
