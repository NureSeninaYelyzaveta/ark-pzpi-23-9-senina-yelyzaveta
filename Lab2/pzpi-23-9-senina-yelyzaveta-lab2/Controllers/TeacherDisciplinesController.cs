using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationArtProgress1.Data;
using WebApplicationArtProgress1.Models;

namespace WebApplicationArtProgress1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherDisciplinesController : ControllerBase
    {
        private readonly ArtProgressContext _context;

        public TeacherDisciplinesController(ArtProgressContext context)
        {
            _context = context;
        }

        // GET: api/TeacherDisciplines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeacherDiscipline>>> GetTeacherDisciplines()
        {
            return await _context.TeacherDisciplines
                .Include(td => td.Teacher)
                .Include(td => td.Discipline)
                .ToListAsync();
        }

        // POST: api/TeacherDisciplines
        [HttpPost]
        public async Task<ActionResult<TeacherDiscipline>> PostTeacherDiscipline(TeacherDiscipline td)
        {
            _context.TeacherDisciplines.Add(td);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTeacherDisciplines), new { td.TeacherID, td.DisciplineID }, td);
        }

        // DELETE: api/TeacherDisciplines/{teacherId}/{disciplineId}
        [HttpDelete("{teacherId}/{disciplineId}")]
        public async Task<IActionResult> DeleteTeacherDiscipline(int teacherId, int disciplineId)
        {
            var td = await _context.TeacherDisciplines.FindAsync(teacherId, disciplineId);
            if (td == null) return NotFound();

            _context.TeacherDisciplines.Remove(td);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
