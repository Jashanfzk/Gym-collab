using GymCollab.Data;
using GymCollab.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymCollab.Controllers
{
    /// <summary>
    /// Controller for managing gym equipment (CRUD operations).
    /// </summary>
    public class EquipmentController : Controller
    {
        private readonly AppDbContext _db;
        public EquipmentController(AppDbContext db) { _db = db; }

        /// <summary>
        /// Displays a list of equipment with optional filtering by category or search query.
        /// </summary>
        public async Task<IActionResult> Index(string? category, string? q)
        {
            var query = _db.Equipment.AsQueryable();
            if (!string.IsNullOrWhiteSpace(category))
            {
                var cat = category.ToLower();
                query = query.Where(e => (e.Category ?? "").ToLower() == cat);
            }
            if (!string.IsNullOrWhiteSpace(q))
            {
                var qLower = q.ToLower();
                query = query.Where(e => e.Name.ToLower().Contains(qLower));
            }
            ViewBag.Categories = await _db.Equipment.Select(e => e.Category!).Distinct().OrderBy(x => x).ToListAsync();
            return View(await query.OrderBy(e => e.Name).ToListAsync());
        }

        /// <summary>
        /// Shows details for a specific equipment item.
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            var item = await _db.Equipment
                .Include(e => e.ClassEquipments).ThenInclude(ce => ce.GymClass)
                .FirstOrDefaultAsync(e => e.EquipmentId == id);
            if (item == null) return NotFound();
            return View(item);
        }

        /// <summary>
        /// Displays the create equipment form.
        /// </summary>
        [Authorize] public IActionResult Create() => View(new Equipment());

        /// <summary>
        /// Handles creation of new equipment.
        /// </summary>
        [Authorize, HttpPost]
        public async Task<IActionResult> Create(Equipment model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.Equipment.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Displays the edit form for a specific equipment item.
        /// </summary>
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _db.Equipment.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        /// <summary>
        /// Handles updates to an existing equipment item.
        /// </summary>
        [Authorize, HttpPost]
        public async Task<IActionResult> Edit(int id, Equipment model)
        {
            if (id != model.EquipmentId) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            _db.Update(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Displays the delete confirmation view for a specific equipment item.
        /// </summary>
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.Equipment.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        /// <summary>
        /// Handles deletion of equipment after confirmation.
        /// </summary>
        [Authorize, HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _db.Equipment.FindAsync(id);
            if (item != null) _db.Equipment.Remove(item);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
