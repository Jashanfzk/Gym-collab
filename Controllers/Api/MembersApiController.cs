using GymCollab.Data;
using GymCollab.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymCollab.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersApiController : ControllerBase
    {
        private readonly AppDbContext _db;
        public MembersApiController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IEnumerable<Member>> List() => await _db.Members.AsNoTracking().ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> Find(int id)
        {
            var m = await _db.Members.FindAsync(id);
            return m == null ? NotFound() : m;
        }

        [Authorize, HttpPost]
        public async Task<ActionResult<Member>> Create(Member input)
        {
            _db.Members.Add(input);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Find), new { id = input.MemberId }, input);
        }

        [Authorize, HttpPost("{id}")]
        public async Task<IActionResult> Update(int id, Member input)
        {
            if (id != input.MemberId) return BadRequest();
            _db.Update(input);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [Authorize, HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var m = await _db.Members.FindAsync(id);
            if (m == null) return NotFound();
            _db.Members.Remove(m);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}/enrollments")]
        public async Task<IEnumerable<ClassEnrollment>> Enrollments(int id)
        {
            return await _db.ClassEnrollments.Where(e => e.MemberId == id)
                .Include(e => e.GymClass).ToListAsync();
        }
    }
}


