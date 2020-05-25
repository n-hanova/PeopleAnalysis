using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleAnalysis.ApplicationAPI;

namespace PeopleAnalysis.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ObjectsController : Controller
    {
        private readonly IApplicationAPIClient applicationAPIClient;

        public ObjectsController(IApplicationAPIClient applicationAPIClient)
        {
            this.applicationAPIClient = applicationAPIClient;
        }

        // GET: AnalysObjects
        public async Task<IActionResult> Index()
        {
            return View(await applicationAPIClient.ApiObjectsAsync());
        }

        // GET: AnalysObjects/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AnalysObject analysObject)
        {
            if (ModelState.IsValid)
            {
                await applicationAPIClient.ApiObjectsCreateAsync(analysObject);
                return RedirectToAction(nameof(Index));
            }
            return View(analysObject);
        }

        // GET: AnalysObjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analysObject = await applicationAPIClient.ApiObjectsFindAsync(id);
            if (analysObject == null)
            {
                return NotFound();
            }
            return View(analysObject);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AnalysObject analysObject)
        {
            if (id != analysObject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await applicationAPIClient.ApiObjectsEditAsync(id, analysObject);
                }
                catch (Exception)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(analysObject);
        }

        // POST: AnalysObjects/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await applicationAPIClient.ApiObjectsDeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}