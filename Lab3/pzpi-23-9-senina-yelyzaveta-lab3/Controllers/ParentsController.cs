using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationArtProgress1.Data;
using WebApplicationArtProgress1.Models;
using WebApplicationArtProgress1.DTOs;

namespace WebApplicationArtProgress1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParentsController : ControllerBase
    {
        private readonly ArtProgressContext _context;

        public ParentsController(ArtProgressContext context)
        {
            _context = context;
        }

        // GET: api/parents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Parent>>> GetParents()
        {
            return await _context.Parents
                .Include(p => p.ParentStudents)
                .Include(p => p.Notifications)
                .ToListAsync();
        }

        // GET: api/parents/{id} 
        [HttpGet("{id}")]
        public async Task<ActionResult<Parent>> GetParent(int id)
        {
            var parent = await _context.Parents
                .Include(p => p.ParentStudents)
                .Include(p => p.Notifications)
                .FirstOrDefaultAsync(p => p.ParentID == id);

            if (parent == null) return NotFound();
            return parent;
        }

        // POST: api/parents
        [HttpPost]
        public async Task<ActionResult<Parent>> PostParent(ParentDto dto)
        {
            var parent = new Parent
            {
                Name = dto.Name,
                ContactInfo = dto.ContactInfo
            };

            _context.Parents.Add(parent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetParent), new { id = parent.ParentID }, parent);
        }

        // PUT: api/parents/{id} 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParent(int id, ParentDto dto)
        {
            var parent = await _context.Parents.FindAsync(id);
            if (parent == null) return NotFound();

            parent.Name = dto.Name;
            parent.ContactInfo = dto.ContactInfo;

            _context.Entry(parent).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/parents/{id} 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParent(int id)
        {
            var parent = await _context.Parents.FindAsync(id);
            if (parent == null) return NotFound();

            _context.Parents.Remove(parent);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}