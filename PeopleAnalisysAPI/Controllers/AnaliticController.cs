using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleAnalisysAPI.ViewModels;
using PeopleAnalysis.Extensions;
using PeopleAnalysis.Services;
using PeopleAnalysis.ViewModels;
using System.Threading.Tasks;

namespace PeopleAnalysis.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AnaliticController : ControllerBase
    {
        private readonly AnaliticService analiticService;
        private readonly IAnaliticAIService analiticAIService;

        public AnaliticController(AnaliticService analiticService, IAnaliticAIService analiticAIService)
        {
            this.analiticService = analiticService;
            this.analiticAIService = analiticAIService;
        }

        [HttpPost("StartAnalys")]
        public ActionResult<bool> StartAnalys([FromBody]AnalitycsRequestModel analitycsRequest, [FromHeader]string authorization)
        {
            return analiticService.CreateRequest(analitycsRequest, User.UserId(), authorization);
        }

        [HttpPost("InProcess")]
        public async Task<ActionResult> InProcess([FromBody]RequestViewModel requestViewModel)
        {
            await analiticAIService.InProcessAsync(requestViewModel, User.Identity.Name);
            return Ok();
        }

        [HttpPost("ReadyResult")]
        public ActionResult ReadyResult([FromBody]ReadyResultViewModel readyResultViewModel)
        {
            analiticAIService.ReadyResult(readyResultViewModel);
            return Ok();
        }
    }
}