using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleAnalysis.Extensions;
using PeopleAnalysis.Models;
using PeopleAnalysis.Services;

namespace PeopleAnalysis.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IDatabaseContext databaseContext;

        public RequestController(IDatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        [HttpGet]
        public ActionResult<List<Request>> GetRequests()
        {
            List<Request> requests = new List<Models.Request>();
            IEnumerable<Request> request = databaseContext.Requests;
            if (!User.IsAdmin())
                request = request.Where(x => x.OwnerId == User.UserId());
            foreach (var r in request.AsEnumerable().GroupBy(x => new { x.UserId, x.Social, x.OwnerId }).ToDictionary(x => x.Key, x => x.OrderBy(x => x.DateTime)))
            {
                var res = r.Value.Last();
                res.DateTime = r.Value.First().DateTime;
                requests.Add(res);
            }
            return requests;
        }

        [HttpPost]
        public ActionResult Delete([FromBody]int toDelete)
        {
            var find = databaseContext.Requests.FirstOrDefault(x => x.Id == toDelete);
            if (find == null)
                return NotFound();
            databaseContext.Add(new Request
            {
                CreateId = User.UserId(),
                OwnerId = find.OwnerId,
                Social = find.Social,
                Status = Status.Closed,
                User = find.User,
                UserId = find.UserId,
                UserUrl = find.UserUrl
            });
            databaseContext.SaveChanges();
            return Ok();
        }
    }
}