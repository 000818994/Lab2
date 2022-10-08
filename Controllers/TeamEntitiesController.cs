// I, Austin Enes, student number 000818994, certify that this material is my
// original work. No other person's work has been used without due
// acknowledgement and I have not made my work available to anyone else.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fall_2022_Lab_1_000818994.Data;
using Fall_2022_Lab_1_000818994.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;



namespace Fall_2022_Lab_1_000818994.Controllers
{
    [Authorize(Roles = "Player,Manager")]
    public class TeamEntitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeamEntitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TeamEntities
        public async Task<IActionResult> Index()
        {
              return View(await _context.TeamEntity.ToListAsync());
        }

        // GET: TeamEntities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TeamEntity == null)
            {
                return NotFound();
            }

            var teamEntity = await _context.TeamEntity
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teamEntity == null)
            {
                return NotFound();
            }

            return View(teamEntity);
        }

        // GET: TeamEntities/Create
        [Authorize(Roles = "Manager")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: TeamEntities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create([Bind("Id,TeamName,EstablishedDate,Email")] TeamEntity teamEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teamEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teamEntity);
        }

        // GET: TeamEntities/Edit/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TeamEntity == null)
            {
                return NotFound();
            }

            var teamEntity = await _context.TeamEntity.FindAsync(id);
            if (teamEntity == null)
            {
                return NotFound();
            }
            return View(teamEntity);
        }

        // POST: TeamEntities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TeamName,EstablishedDate,Email")] TeamEntity teamEntity)
        {
            if (id != teamEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teamEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamEntityExists(teamEntity.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(teamEntity);
        }

        // GET: TeamEntities/Delete/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TeamEntity == null)
            {
                return NotFound();
            }

            var teamEntity = await _context.TeamEntity
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teamEntity == null)
            {
                return NotFound();
            }

            return View(teamEntity);
        }

        // POST: TeamEntities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TeamEntity == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TeamEntity'  is null.");
            }
            var teamEntity = await _context.TeamEntity.FindAsync(id);
            if (teamEntity != null)
            {
                _context.TeamEntity.Remove(teamEntity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamEntityExists(int id)
        {
          return _context.TeamEntity.Any(e => e.Id == id);
        }
    }
}
