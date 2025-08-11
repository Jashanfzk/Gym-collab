using GymCollab.Data;
using GymCollab.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymCollab.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class GymClassesApiController : ControllerBase
    {
        private readonly AppDbContext _db;
        public GymClassesApiController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GymClass>>> List() => await _db.GymClasses.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<GymClass>> Find(int id)
        {
            var cls = await _db.GymClasses.FindAsync(id);
            return cls == null ? NotFound() : cls;
        }

        [Authorize, HttpPost]
        public async Task<ActionResult<GymClass>> Create(GymClass input)
        {
            _db.GymClasses.Add(input);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Find), new { id = input.GymClassId }, input);
        }

        [Authorize, HttpPost("{id}")]
        public async Task<IActionResult> Update(int id, GymClass input)
        {
            if (id != input.GymClassId) return BadRequest();
            _db.Update(input);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [Authorize, HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cls = await _db.GymClasses.FindAsync(id);
            if (cls == null) return NotFound();
            _db.GymClasses.Remove(cls);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}/equipment")]
        public async Task<ActionResult<IEnumerable<ClassEquipment>>> ListEquipment(int id)
        {
            var list = await _db.ClassEquipments.Where(ce => ce.GymClassId == id)
                .Include(ce => ce.Equipment).ToListAsync();
            return list;
        }

        [Authorize, HttpPost("{id}/equipment")]
        public async Task<IActionResult> AddEquipment(int id, [FromBody] ClassEquipment input)
        {
            input.GymClassId = id;
            _db.ClassEquipments.Add(input);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [Authorize, HttpDelete("{id}/equipment/{equipmentId}")]
        public async Task<IActionResult> RemoveEquipment(int id, int equipmentId)
        {
            var ce = await _db.ClassEquipments.FirstOrDefaultAsync(x => x.GymClassId == id && x.EquipmentId == equipmentId);
            if (ce == null) return NotFound();
            _db.ClassEquipments.Remove(ce);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
