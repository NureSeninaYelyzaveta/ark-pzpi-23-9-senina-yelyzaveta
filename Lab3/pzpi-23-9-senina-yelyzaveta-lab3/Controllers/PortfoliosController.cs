using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationArtProgress1.Data;
using WebApplicationArtProgress1.Models;
using WebApplicationArtProgress1.DTOs;

namespace WebApplicationArtProgress1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfoliosController : ControllerBase
    {
        private readonly ArtProgressContext _context;

        public PortfoliosController(ArtProgressContext context)
        {
            _context = context;
        }

        // GET: api/portfolios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Portfolio>>> GetPortfolios()
        {
            return await _context.Portfolios
                .Include(p => p.Student)
                .ToListAsync();
        }

        // GET: api/portfolios/{id} 
        [HttpGet("{id}")]
        public async Task<ActionResult<Portfolio>> GetPortfolio(int id)
        {
            var portfolio = await _context.Portfolios
                .Include(p => p.Student)
                .FirstOrDefaultAsync(p => p.PortfolioID == id);

            if (portfolio == null) return NotFound();
            return portfolio;
        }

        // POST: api/portfolios
        [HttpPost]
        public async Task<ActionResult<Portfolio>> PostPortfolio(PortfolioDto dto)
        {
            var portfolio = new Portfolio
            {
                Files = dto.Files,
                StudentID = dto.StudentID
            };

            _context.Portfolios.Add(portfolio);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPortfolio), new { id = portfolio.PortfolioID }, portfolio);
        }

        // PUT: api/portfolios/{id} 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPortfolio(int id, PortfolioDto dto)
        {
            var portfolio = await _context.Portfolios.FindAsync(id);
            if (portfolio == null) return NotFound();

            portfolio.Files = dto.Files;
            portfolio.StudentID = dto.StudentID;

            _context.Entry(portfolio).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/portfolios/{id} 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePortfolio(int id)
        {
            var portfolio = await _context.Portfolios.FindAsync(id);
            if (portfolio == null) return NotFound();

            _context.Portfolios.Remove(portfolio);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}