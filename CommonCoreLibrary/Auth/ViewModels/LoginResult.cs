namespace AuthAPI.Models.Controller
{
    public class LoginResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
