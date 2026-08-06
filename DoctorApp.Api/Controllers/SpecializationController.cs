using DoctorApp.DB.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoctorApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationController : ControllerBase
    {
        private readonly DoctorAppDbContext _context;

        public SpecializationController(DoctorAppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSpecializations()
        {
            var specializations = await _context.Specializations.ToListAsync();

            return Ok(specializations);
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                var allSpecializations = await _context.Specializations.ToListAsync();
                return Ok(allSpecializations);
            }

            var result = await _context.Specializations
                .Where(s => s.Name.Contains(keyword))
                .ToListAsync();

            return Ok(result);
        }
    }
}