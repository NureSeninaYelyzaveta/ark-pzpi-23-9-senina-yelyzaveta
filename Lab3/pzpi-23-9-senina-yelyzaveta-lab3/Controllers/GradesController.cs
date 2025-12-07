using Microsoft.AspNetCore.Mvc;
using WebApplicationArtProgress1.DTOs;
using WebApplicationArtProgress1.Models;
using WebApplicationArtProgress1.Services;

namespace WebApplicationArtProgress1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradesController : ControllerBase
    {
        private readonly GradesService _gradesService;

        public GradesController(GradesService gradesService)
        {
            _gradesService = gradesService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Grade>>> GetGrades()
        {
            return Ok(await _gradesService.GetAllGradesAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Grade>> GetGrade(int id)
        {
            var grade = await _gradesService.GetGradeAsync(id);
            if (grade == null) return NotFound();
            return Ok(grade);
        }

        [HttpPost]
        public async Task<ActionResult<Grade>> PostGrade(GradeDto dto)
        {
            var grade = await _gradesService.AddGradeAsync(
                dto.StudentID,
                dto.TeacherID,
                dto.DisciplineID,
                dto.Value,
                null
            );

            return CreatedAtAction(nameof(GetGrade), new { id = grade.GradeID }, grade);
        }

        //  PUT: оновити оцінку
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGrade(int id, GradeDto dto)
        {
            var updated = await _gradesService.UpdateGradeAsync(id, dto);
            if (!updated) return NotFound();
            return NoContent();
        }

        //  DELETE: видалити оцінку
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrade(int id)
        {
            var deleted = await _gradesService.DeleteGradeAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}