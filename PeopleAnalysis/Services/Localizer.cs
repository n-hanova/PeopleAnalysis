using CommonCoreLibrary.Extensions;
using CommonCoreLibrary.Services;
using Microsoft.AspNetCore.Http;
using System.Threading;

namespace PeopleAnalysis.Services
{
    public class Localizer : ILocalizer
    {
        public Localizer(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(httpContextAccessor.HttpContext.User.UICode());
        }

        public string this[string code] => Properties.Resources.ResourceManager.GetString(code);
    }
}
