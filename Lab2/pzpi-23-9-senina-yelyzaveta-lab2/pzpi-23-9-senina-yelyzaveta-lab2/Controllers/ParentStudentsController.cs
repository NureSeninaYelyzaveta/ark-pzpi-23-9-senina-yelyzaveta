using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationArtProgress1.Data;
using WebApplicationArtProgress1.Models;

namespace WebApplicationArtProgress1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParentStudentsController : ControllerBase
    {
        private readonly ArtProgressContext _context;

        public ParentStudentsController(ArtProgressContext context)
        {
            _context = context;
        }

        // GET: api/ParentStudents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParentStudent>>> GetParentStudents()
        {
            return await _context.ParentStudents
                .Include(ps => ps.Parent)
                .Include(ps => ps.Student)
                .ToListAsync();
        }

        // POST: api/ParentStudents
        [HttpPost]
        public async Task<ActionResult<ParentStudent>> PostParentStudent(ParentStudent ps)
        {
            _context.ParentStudents.Add(ps);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetParentStudents), new { ps.ParentID, ps.StudentID }, ps);
        }

        // DELETE: api/ParentStudents/{parentId}/{studentId}
        [HttpDelete("{parentId}/{studentId}")]
        public async Task<IActionResult> DeleteParentStudent(int parentId, int studentId)
        {
            var ps = await _context.ParentStudents.FindAsync(parentId, studentId);
            if (ps == null) return NotFound();

            _context.ParentStudents.Remove(ps);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}