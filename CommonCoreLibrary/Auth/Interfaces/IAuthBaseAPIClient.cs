using AuthAPI.Models.Controller;
using CommonCoreLibrary.Auth.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CommonCoreLibrary.APIClient
{
    public interface IAuthBaseAPIClient
    {
        Task AuthLoginGetAsync();
        Task<IAuthResult> AuthRefreshtokenAsync(string v1, string v2);
    }
}
