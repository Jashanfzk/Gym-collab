using GymCollab.Data;
using GymCollab.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymCollab.Controllers
{
    public class MembersController : Controller
    {
        private readonly AppDbContext _db;
        public MembersController(AppDbContext db) { _db = db; }

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

        public async Task<IActionResult> Details(int id)
        {
            var m = await _db.Members
                .Include(x => x.Enrollments).ThenInclude(e => e.GymClass)
                .FirstOrDefaultAsync(m => m.MemberId == id);
            if (m == null) return NotFound();
            return View(m);
        }

        [Authorize] public IActionResult Create() => View(new Member());

        [Authorize, HttpPost]
        public async Task<IActionResult> Create(Member model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.Members.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var m = await _db.Members.FindAsync(id);
            if (m == null) return NotFound();
            return View(m);
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> Edit(int id, Member model)
        {
            if (id != model.MemberId) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            _db.Update(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var m = await _db.Members.FindAsync(id);
            if (m == null) return NotFound();
            return View(m);
        }

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


