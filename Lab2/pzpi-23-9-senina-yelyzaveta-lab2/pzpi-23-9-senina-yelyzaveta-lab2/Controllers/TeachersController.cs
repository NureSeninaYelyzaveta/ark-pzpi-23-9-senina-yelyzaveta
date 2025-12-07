using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationArtProgress1.Data;
using WebApplicationArtProgress1.Models;
using WebApplicationArtProgress1.DTOs;

namespace WebApplicationArtProgress1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly ArtProgressContext _context;

        public TeachersController(ArtProgressContext context)
        {
            _context = context;
        }

        // GET: api/teachers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachers()
        {
            return await _context.Teachers.ToListAsync();
        }

        // GET: api/teachers/{id} 
        [HttpGet("{id}")]
        public async Task<ActionResult<Teacher>> GetTeacher(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return NotFound();
            return teacher;
        }

        // POST: api/teachers
        [HttpPost]
        public async Task<ActionResult<Teacher>> PostTeacher(TeacherDto dto)
        {
            var teacher = new Teacher
            {
                Name = dto.Name,
                Position = dto.Position,
                ContactInfo = dto.ContactInfo
            };

            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTeacher), new { id = teacher.TeacherID }, teacher);
        }

        // PUT: api/teachers/{id} 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacher(int id, TeacherDto dto)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return NotFound();

            teacher.Name = dto.Name;
            teacher.Position = dto.Position;
            teacher.ContactInfo = dto.ContactInfo;

            _context.Entry(teacher).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/teachers/{id} 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return NotFound();

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
