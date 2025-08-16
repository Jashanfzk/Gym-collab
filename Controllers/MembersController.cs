using GymCollab.Data;
using GymCollab.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymCollab.Controllers
{
    /// <summary>
    /// Controller for managing gym members.
    /// </summary>
    public class MembersController : Controller
    {
        private readonly AppDbContext _db;
        public MembersController(AppDbContext db) { _db = db; }

        /// <summary>
        /// Displays a list of members, optionally filtered by search query.
        /// </summary>
        public async Task<IActionResult> Index(string? q)
        {
            var query = _db.Members.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
            {
                var qLower = q.ToLower();
                query = query.Where(m => m.FullName.ToLower().Contains(qLower));
            }
            return View(await query.OrderBy(m => m.FullName).ToListAsync());
        }

        /// <summary>
        /// Shows details of a specific member including their class enrollments.
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            var m = await _db.Members
                .Include(x => x.Enrollments).ThenInclude(e => e.GymClass)
                .FirstOrDefaultAsync(m => m.MemberId == id);
            if (m == null) return NotFound();
            return View(m);
        }

        /// <summary>
        /// Displays the form for creating a new member.
        /// </summary>
        [Authorize] public IActionResult Create() => View(new Member());

        /// <summary>
        /// Handles submission of a new member.
        /// </summary>
        [Authorize, HttpPost]
        public async Task<IActionResult> Create(Member model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.Members.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Displays the form for editing an existing member.
        /// </summary>
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var m = await _db.Members.FindAsync(id);
            if (m == null) return NotFound();
            return View(m);
        }

        /// <summary>
        /// Handles submission of changes to an existing member.
        /// </summary>
        [Authorize, HttpPost]
        public async Task<IActionResult> Edit(int id, Member model)
        {
            if (id != model.MemberId) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            _db.Update(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Displays the delete confirmation view for a member.
        /// </summary>
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var m = await _db.Members.FindAsync(id);
            if (m == null) return NotFound();
            return View(m);
        }

        /// <summary>
        /// Handles deletion of a member after confirmation.
        /// </summary>
        [Authorize, HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var m = await _db.Members.FindAsync(id);
            if (m != null) _db.Members.Remove(m);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
