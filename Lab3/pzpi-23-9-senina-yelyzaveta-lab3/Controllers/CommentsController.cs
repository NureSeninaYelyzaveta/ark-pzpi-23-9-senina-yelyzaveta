using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationArtProgress1.Data;
using WebApplicationArtProgress1.Models;
using WebApplicationArtProgress1.DTOs;

namespace WebApplicationArtProgress1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ArtProgressContext _context;  // підключення до бази даних

        public CommentsController(ArtProgressContext context)
        {
            _context = context;
        }

        // GET: api/comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            return await _context.Comments
                .Include(c => c.Student)
                .Include(c => c.Teacher)
                .ToListAsync();
        }

        // GET: api/comments/{id} 
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            // отримуємо конкретний коментар за id
            var comment = await _context.Comments
                .Include(c => c.Student)
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(c => c.CommentID == id);

            if (comment == null) return NotFound(); // якщо немає повертаємо NotFound
            return comment;
        }

        // POST: api/comments
        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment(CommentDto dto)
        {
            // створюємо новий коментар з даних DTO
            var comment = new Comment
            {
                Text = dto.Text,
                Date = dto.Date,
                StudentID = dto.StudentID,
                TeacherID = dto.TeacherID
            };

            _context.Comments.Add(comment); // додаємо до бази
            await _context.SaveChangesAsync(); // зберігаємо

            return CreatedAtAction(nameof(GetComment), new { id = comment.CommentID }, comment);
        }

        // PUT: api/comments/{id} 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, CommentDto dto)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return NotFound();

            comment.Text = dto.Text;
            comment.Date = dto.Date;
            comment.StudentID = dto.StudentID;
            comment.TeacherID = dto.TeacherID;

            _context.Entry(comment).State = EntityState.Modified;  // позначаємо як змінений
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/comments/{id} 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return NotFound();

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}