using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleAnalysis.ApplicationAPI;
using PeopleAnalysis.Extensions;
using PeopleAnalysis.Models;
using PeopleAnalysis.Services;

namespace PeopleAnalysis.Controllers
{
    [Authorize]
    public class RequestController : Controller
    {
        private readonly IApplicationAPIClient applicationAPIClient;

        public RequestController(IApplicationAPIClient applicationAPIClient)
        {
            this.applicationAPIClient = applicationAPIClient;
        }

        public async Task<IActionResult> Index()
        {
            List<Request> requests = new List<Request>();
            IEnumerable<Request> request = await applicationAPIClient.ApiRequestGetAsync();
            if (!User.IsAdmin())
                request = request.Where(x => x.OwnerId == User.UserId());
            foreach (var r in request.AsEnumerable().GroupBy(x => new { x.UserId, x.Social, x.OwnerId }).ToDictionary(x => x.Key, x => x.OrderBy(x => x.DateTime)))
            {
                var res = r.Value.Last();
                res.DateTime = r.Value.First().DateTime;
                requests.Add(res);
            }
            return View(requests);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm]int toDelete)
        {
            await applicationAPIClient.ApiRequestPostAsync(toDelete);
            return Redirect("Index");
        }
    }
}