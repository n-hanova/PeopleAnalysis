using System.Linq;
using System.Threading.Tasks;
using CommonCoreLibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleAnalysis.AuthAPI;

namespace PeopleAnalysis.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IAuthAPIClient authAPIClient;

        public UsersController(IAuthAPIClient authAPIClient)
        {
            this.authAPIClient = authAPIClient;
        }

        // GET: LoginViewModels
        public async Task<IActionResult> Index()
        {
            return View(await authAPIClient.ApiUsersAsync());
        }

        // GET: LoginViewModels/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                await authAPIClient.ApiUsersCreateAsync(loginViewModel);
                return RedirectToAction(nameof(Index));
            }
            return View(loginViewModel);
        }

        // GET: LoginViewModels/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
                return NotFound();
            return View(await authAPIClient.ApiUsersFindAsync(id));
        }

        [HttpPost]

        public async Task<IActionResult> Edit(string id, UserViewModel loginViewModel)
        {
            if (id != loginViewModel.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                await authAPIClient.ApiUsersEditAsync(id, loginViewModel);
                return RedirectToAction(nameof(Index));
            }
            return View(loginViewModel);
        }

        // POST: LoginViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed([FromForm]string toDelete)
        {
            await authAPIClient.ApiUsersDeleteAsync(toDelete);
            return RedirectToAction(nameof(Index));
        }
    }
}