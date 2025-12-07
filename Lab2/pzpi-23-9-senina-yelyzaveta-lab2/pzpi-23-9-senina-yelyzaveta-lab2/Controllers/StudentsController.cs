using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationArtProgress1.Data;
using WebApplicationArtProgress1.Models;
using WebApplicationArtProgress1.DTOs;

namespace WebApplicationArtProgress1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ArtProgressContext _context;

        public StudentsController(ArtProgressContext context)
        {
            _context = context;
        }

        // GET: api/students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        // GET: api/students/{id} 
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();
            return student;
        }

        // POST: api/students
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(StudentDto dto)
        {
            var student = new Student
            {
                Name = dto.Name,
                Group = dto.Group,
                ContactInfo = dto.ContactInfo
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudent), new { id = student.StudentID }, student);
        }

        // PUT: api/students/{id} 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, StudentDto dto)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            student.Name = dto.Name;
            student.Group = dto.Group;
            student.ContactInfo = dto.ContactInfo;

            _context.Entry(student).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/students/{id} 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
