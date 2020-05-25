using System;
using System.Collections.Generic;
using System.Text;

namespace CommonCoreLibrary.Auth.Interfaces
{
    public interface IAuthResult
    {
        string AccessToken { get; set; }
        string RefreshToken { get; set; }
    }

    public class BaseAuthResult : IAuthResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
