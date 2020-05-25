using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleAnalysis.ApplicationAPI;
using System.Threading.Tasks;

namespace PeopleAnalysis.Controllers
{
    [Authorize]
    public class AnaliticController : Controller
    {
        private readonly IApplicationAPIClient applicationAPIClient;

        public AnaliticController(IApplicationAPIClient applicationAPIClient)
        {
            this.applicationAPIClient = applicationAPIClient;
        }

        public async Task<IActionResult> StartAnalys([FromForm]AnalitycsRequestModel analitycsRequest, [FromHeader]string authorization)
        {
            if (await applicationAPIClient.ApiAnaliticStartanalysAsync(string.Empty, analitycsRequest))
                return RedirectToActionPermanent("Index", "Request");
            return BadRequest();
        }
    }
}