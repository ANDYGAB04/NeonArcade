using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeonArcade.Server.Data;

namespace NeonArcade.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GamesController(ApplicationDbContext context)
        {
            _context = context;
        }

        //GET: /Games
        [HttpGet]
        public async Task<IActionResult> GetGames()
        {
            var games = await _context.Games.ToListAsync();
            return Ok(games);
        }

        //GET: /Games/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGame(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return Ok(game);
        }
    }
}
