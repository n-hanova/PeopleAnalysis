using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PeopleAnalysis.Models;
using PeopleAnalysis.Services;
using PeopleAnalysis.ViewModels;

namespace PeopleAnalysis.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FindPeopleController : ControllerBase
    {
        private readonly ILogger<FindPeopleController> _logger;
        private readonly ApisManager apisManager;

        public FindPeopleController(ILogger<FindPeopleController> logger, ApisManager apisManager)
        {
            _logger = logger;
            this.apisManager = apisManager;
        }

        [HttpPost]
        public ActionResult<FindPeoplePageViewModel> FindPeople([FromBody]FindPeoplePageViewModel findPeoplePageViewModel)
        {
            if (!string.IsNullOrEmpty(findPeoplePageViewModel.FindPeopleViewModel.FindText))
                findPeoplePageViewModel.FinderResultViewModel = apisManager.GetFinded(findPeoplePageViewModel.FindPeopleViewModel.FindText);
            return findPeoplePageViewModel;
        }
    }
}
