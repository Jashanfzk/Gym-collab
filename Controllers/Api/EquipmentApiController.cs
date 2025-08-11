using GymCollab.Data;
using GymCollab.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymCollab.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipmentApiController : ControllerBase
    {
        private readonly AppDbContext _db;
        public EquipmentApiController(AppDbContext db) { _db = db; }

        [HttpGet] public async Task<IEnumerable<Equipment>> List() => await _db.Equipment.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Equipment>> Find(int id)
        {
            var item = await _db.Equipment.FindAsync(id);
            return item == null ? NotFound() : item;
        }

        [Authorize, HttpPost]
        public async Task<ActionResult<Equipment>> Create(Equipment input)
        {
            _db.Equipment.Add(input);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Find), new { id = input.EquipmentId }, input);
        }

        [Authorize, HttpPost("{id}")]
        public async Task<IActionResult> Update(int id, Equipment input)
        {
            if (id != input.EquipmentId) return BadRequest();
            _db.Update(input);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [Authorize, HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.Equipment.FindAsync(id);
            if (item == null) return NotFound();
            _db.Equipment.Remove(item);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}/classes")]
        public async Task<IEnumerable<ClassEquipment>> ListClasses(int id)
        {
            return await _db.ClassEquipments.Where(ce => ce.EquipmentId == id)
                .Include(ce => ce.GymClass).ToListAsync();
        }
    }
}
