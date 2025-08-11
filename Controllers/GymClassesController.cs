using GymCollab.Data;
using GymCollab.Models;
using GymCollab.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymCollab.Controllers
{
    public class GymClassesController : Controller
    {
        private readonly AppDbContext _db;
        public GymClassesController(AppDbContext db) { _db = db; }

        // GET: /GymClasses
        public async Task<IActionResult> Index(string? q)
        {
            var query = _db.GymClasses.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
            {
                var qLower = q.ToLower();
                query = query.Where(c => c.Name.ToLower().Contains(qLower));
            }
            var list = await query.OrderBy(c => c.DayOfWeek).ThenBy(c => c.Time).ToListAsync();
            return View(list);
        }

        // GET: /GymClasses/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var cls = await _db.GymClasses
                .Include(c => c.ClassEquipments).ThenInclude(ce => ce.Equipment)
                .FirstOrDefaultAsync(c => c.GymClassId == id);
            if (cls == null) return NotFound();
            var vm = new ClassDetailsVM
            {
                GymClass = cls,
                Assigned = cls.ClassEquipments.ToList(),
                Available = await _db.Equipment.OrderBy(e => e.Name).ToListAsync()
            };
            return View(vm);
        }

        [Authorize]
        public IActionResult Create() => View(new GymClass());

        [Authorize, HttpPost]
        public async Task<IActionResult> Create(GymClass model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.GymClasses.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var cls = await _db.GymClasses.FindAsync(id);
            if (cls == null) return NotFound();
            return View(cls);
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> Edit(int id, GymClass model)
        {
            if (id != model.GymClassId) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            _db.Update(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var cls = await _db.GymClasses.FindAsync(id);
            if (cls == null) return NotFound();
            return View(cls);
        }

        [Authorize, HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cls = await _db.GymClasses.FindAsync(id);
            if (cls != null) _db.GymClasses.Remove(cls);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Related CRUD: Add equipment to class
        [Authorize, HttpPost]
        public async Task<IActionResult> AddEquipment(int id, ClassDetailsVM vm)
        {
            var cls = await _db.GymClasses.FindAsync(id);
            var eq = await _db.Equipment.FindAsync(vm.EquipmentId);
            if (cls == null || eq == null) return NotFound();

            var existing = await _db.ClassEquipments
                .FirstOrDefaultAsync(x => x.GymClassId == id && x.EquipmentId == vm.EquipmentId);
            if (existing == null)
            {
                existing = new ClassEquipment { GymClassId = id, EquipmentId = vm.EquipmentId, QuantityUsed = vm.Quantity };
                _db.ClassEquipments.Add(existing);
            }
            else
            {
                existing.QuantityUsed = vm.Quantity;
                _db.ClassEquipments.Update(existing);
            }
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize]
        public async Task<IActionResult> RemoveEquipment(int id, int equipmentId)
        {
            var ce = await _db.ClassEquipments
                .FirstOrDefaultAsync(x => x.GymClassId == id && x.EquipmentId == equipmentId);
            if (ce != null) { _db.ClassEquipments.Remove(ce); await _db.SaveChangesAsync(); }
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
