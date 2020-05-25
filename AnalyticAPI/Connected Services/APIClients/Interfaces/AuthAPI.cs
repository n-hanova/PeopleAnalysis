using CommonCoreLibrary.APIClient;
using CommonCoreLibrary.Auth.Interfaces;

namespace AnalyticAPI.AuthAPI
{
    public partial interface IAuthAPIClient : IAuthBaseAPIClient
    {
    }

    public partial class LoginResult : IAuthResult
    {
    }
}
