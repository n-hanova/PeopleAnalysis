using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleAnalysis.ApplicationAPI;
using PeopleAnalysis.Models;

namespace PeopleAnalysis.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IApplicationAPIClient applicationAPIClient;

        public HomeController(IApplicationAPIClient applicationAPIClient)
        {
            this.applicationAPIClient = applicationAPIClient;
        }

        public IActionResult Index()
        {
            return View(new FindPeoplePageViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm]FindPeoplePageViewModel findPeoplePageViewModel)
        {
            if (!string.IsNullOrEmpty(findPeoplePageViewModel.FindPeopleViewModel.FindText))
                findPeoplePageViewModel = await applicationAPIClient.ApiFindpeopleAsync(findPeoplePageViewModel);
            return View(findPeoplePageViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
