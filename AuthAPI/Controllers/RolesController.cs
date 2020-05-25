using System.Collections.Generic;
using System.Linq;
using AuthAPI.Models.Controller;
using AuthAPI.Services;
using CommonCoreLibrary.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IAuthDataProvider authDataProvider;
        private readonly IMapperService mapperService;

        public RolesController(IAuthDataProvider authDataProvider, IMapperService mapperService)
        {
            this.authDataProvider = authDataProvider;
            this.mapperService = mapperService;
        }

        [HttpGet]
        public ActionResult<List<RoleModel>> Roles()
        {
            return authDataProvider.Roles.Select(x => mapperService.Map<RoleModel>(x)).ToList();
        }
    }
}