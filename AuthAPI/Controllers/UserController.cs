using AuthAPI.Models.Controller;
using AuthAPI.Services;
using CommonCoreLibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IMapperService mapperService;
        private readonly IAuthDataProvider authDataProvider;

        public UserController(IUserService userService, IMapperService mapperService, IAuthDataProvider authDataProvider)
        {
            this.userService = userService;
            this.mapperService = mapperService;
            this.authDataProvider = authDataProvider;
        }

        [HttpGet]
        public ActionResult<UserModel> GetUser([FromHeader]string authorization)
        {
            var parts = authorization.Split(" ");
            return mapperService.Map<UserModel>(userService.GetUser(parts[1]));
        }

        [HttpGet("Find")]
        public ActionResult<UserModel> FindUser(string email)
        {
            var user = authDataProvider.Users.FirstOrDefault(x => x.Email == email);
            if (user == null)
                return NotFound();
            return mapperService.Map<UserModel>(user);
        }

        [HttpGet("lang")]
        public ActionResult<IEnumerable<LanguageModel>> Languages() => authDataProvider.Languages.Select(x => mapperService.Map<LanguageModel>(x)).ToList();

        [HttpPut("changelanguage")]
        public ActionResult<UserModel> UpdateLanguage([FromBody]ChangeLanguageModel changeLanguageModel, [FromHeader]string authorization)
        {
            var parts = authorization.Split(" ");
            var user = userService.GetUser(parts[1]);
            user.Language = authDataProvider.Languages.FirstOrDefault(x => x.Code == changeLanguageModel.Language);
            authDataProvider.Update(user);
            authDataProvider.SaveChanges();
            return mapperService.Map<UserModel>(user);
        }
    }
}