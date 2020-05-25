using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleAnalysis.Models;
using PeopleAnalysis.Services;

namespace PeopleAnalysis.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ObjectsController : ControllerBase
    {
        private readonly IDatabaseContext _context;

        public ObjectsController(IDatabaseContext context)
        {
            _context = context;
        }

        // GET: AnalysObjects
        [HttpGet]
        public async Task<ActionResult<List<AnalysObject>>> GetObjects()
        {
            return await _context.AnalysObjects.ToListAsync();
        }

        [HttpGet("Find")]
        public async Task<ActionResult<AnalysObject>> FindObject(int id)
        {
            return await _context.AnalysObjects.FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<AnalysObject>> Create(AnalysObject analysObject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(analysObject);
                await _context.SaveChangesAsync();
                return analysObject;
            }
            return BadRequest();
        }

        [HttpPut("Edit")]
        public async Task<ActionResult<AnalysObject>> Edit(int id, AnalysObject analysObject)
        {
            if (id != analysObject.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(analysObject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnalysObjectExists(analysObject.Id))
                        return NotFound();
                    else
                        throw;
                }
                return Ok();
            }
            return analysObject;
        }


        [HttpDelete("Delete")]
        public async Task<ActionResult<AnalysObject>> DeleteConfirmed(int id)
        {
            var analysObject = await _context.AnalysObjects.FirstOrDefaultAsync(x => x.Id == id);
            _context.Remove(analysObject);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool AnalysObjectExists(int id) => _context.AnalysObjects.Any(e => e.Id == id);
    }
}