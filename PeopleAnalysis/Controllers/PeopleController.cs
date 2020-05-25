using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleAnalysis.ApplicationAPI;
using System.Threading.Tasks;

namespace PeopleAnalysis.Controllers
{
    [Authorize]
    public class PeopleController : Controller
    {
        private readonly IApplicationAPIClient applicationAPIClient;

        public PeopleController(IApplicationAPIClient applicationAPIClient)
        {
            this.applicationAPIClient = applicationAPIClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery]OpenPeopleViewModel openPeopleViewModel)
        {
            return View(await applicationAPIClient.ApiPeopleAsync(openPeopleViewModel));
        }
    }
}