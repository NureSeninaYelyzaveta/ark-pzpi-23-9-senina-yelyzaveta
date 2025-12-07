using Microsoft.AspNetCore.Mvc;
using WebApplicationArtProgress1.Services;

namespace WebApplicationArtProgress1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _adminService;

        public AdminController(AdminService adminService)
        {
            _adminService = adminService;  // підключаємо сервіс через DI
        }

        [HttpPost("disciplines")]
        public async Task<IActionResult> AddDiscipline([FromBody] string name)
        {
            var discipline = await _adminService.AddDisciplineAsync(name);  // додаємо нову дисципліну
            return Ok(discipline);  // повертаємо результат
        }

        [HttpDelete("disciplines/{id}")]
        public async Task<IActionResult> RemoveDiscipline(int id)
        {
            var result = await _adminService.RemoveDisciplineAsync(id);  // видаляємо дисципліну за id
            return result ? NoContent() : NotFound();
        }

        [HttpGet("systemstats")]
        public async Task<IActionResult> GetSystemStats()
        {
            var stats = await _adminService.GetSystemStatsAsync();  // отримуємо статистику системи
            return Ok(stats);
        }
    }
}
