using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationArtProgress1.Data;
using WebApplicationArtProgress1.Models;
using WebApplicationArtProgress1.DTOs;

namespace WebApplicationArtProgress1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisciplinesController : ControllerBase
    {
        private readonly ArtProgressContext _context;

        public DisciplinesController(ArtProgressContext context)
        {
            _context = context;
        }

        // GET: api/disciplines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Discipline>>> GetDisciplines()
        {
            return await _context.Disciplines.ToListAsync();
        }

        // GET: api/disciplines/{id} 
        [HttpGet("{id}")]
        public async Task<ActionResult<Discipline>> GetDiscipline(int id)
        {
            var discipline = await _context.Disciplines.FindAsync(id);
            if (discipline == null) return NotFound();
            return discipline;
        }

        // POST: api/disciplines
        [HttpPost]
        public async Task<ActionResult<Discipline>> PostDiscipline(DisciplineDto dto)
        {
            var discipline = new Discipline
            {
                Name = dto.Name,
                Description = dto.Description
            };

            _context.Disciplines.Add(discipline);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDiscipline), new { id = discipline.DisciplineID }, discipline);
        }

        // PUT: api/disciplines/{id} 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDiscipline(int id, DisciplineDto dto)
        {
            var discipline = await _context.Disciplines.FindAsync(id);
            if (discipline == null) return NotFound();

            discipline.Name = dto.Name;
            discipline.Description = dto.Description;

            _context.Entry(discipline).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/disciplines/{id} 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiscipline(int id)
        {
            var discipline = await _context.Disciplines.FindAsync(id);
            if (discipline == null) return NotFound();

            _context.Disciplines.Remove(discipline);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}