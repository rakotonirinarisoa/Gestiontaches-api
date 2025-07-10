using Gestion_de_Tâches.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gestion_de_Tâches.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class GestionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GestionController(AppDbContext context)
        {
            _context = context;
        }

        // Exemple : une action globale, par exemple un dashboard ou stats
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var totalUsers = await _context.Users.CountAsync();
            var totalTasks = await _context.Tasks.CountAsync();

            return Ok(new { totalUsers, totalTasks });
        }
    }
}
