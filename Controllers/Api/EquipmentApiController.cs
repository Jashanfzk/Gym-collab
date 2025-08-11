using GymCollab.Data;
using GymCollab.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymCollab.Controllers.Api
{
    /// <summary>
    /// API controller for managing gym equipment
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EquipmentApiController : ControllerBase
    {
        private readonly AppDbContext _db;
        public EquipmentApiController(AppDbContext db) { _db = db; }

        /// <summary>
        /// Retrieves all equipment items
        /// </summary>
        /// <returns>A list of all equipment</returns>
        [HttpGet] 
        public async Task<IEnumerable<Equipment>> List() => await _db.Equipment.ToListAsync();

        /// <summary>
        /// Finds equipment by ID
        /// </summary>
        /// <param name="id">The equipment ID</param>
        /// <returns>The equipment item if found, otherwise NotFound</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Equipment>> Find(int id)
        {
            var item = await _db.Equipment.FindAsync(id);
            return item == null ? NotFound() : item;
        }

        /// <summary>
        /// Creates a new equipment item
        /// </summary>
        /// <param name="input">The equipment data</param>
        /// <returns>The created equipment with its ID</returns>
        [Authorize, HttpPost]
        public async Task<ActionResult<Equipment>> Create(Equipment input)
        {
            _db.Equipment.Add(input);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Find), new { id = input.EquipmentId }, input);
        }

        /// <summary>
        /// Updates an existing equipment item
        /// </summary>
        /// <param name="id">The equipment ID</param>
        /// <param name="input">The updated equipment data</param>
        /// <returns>No content on success</returns>
        [Authorize, HttpPost("{id}")]
        public async Task<IActionResult> Update(int id, Equipment input)
        {
            if (id != input.EquipmentId) return BadRequest();
            _db.Update(input);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Deletes an equipment item
        /// </summary>
        /// <param name="id">The equipment ID to delete</param>
        /// <returns>No content on success</returns>
        [Authorize, HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.Equipment.FindAsync(id);
            if (item == null) return NotFound();
            _db.Equipment.Remove(item);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Lists all gym classes that use a specific equipment
        /// </summary>
        /// <param name="id">The equipment ID</param>
        /// <returns>A list of class equipment relationships</returns>
        [HttpGet("{id}/classes")]
        public async Task<IEnumerable<ClassEquipment>> ListClasses(int id)
        {
            return await _db.ClassEquipments.Where(ce => ce.EquipmentId == id)
                .Include(ce => ce.GymClass).ToListAsync();
        }
    }
}
