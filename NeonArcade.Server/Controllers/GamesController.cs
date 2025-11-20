using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeonArcade.Server.Data;
using NeonArcade.Server.Models;

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

        //POST: /Games
        [HttpPost]
        public async Task<ActionResult<Game>> CreateGame([FromBody] Game game)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            game.Id = 0;
            game.CreatedAt = DateTimeOffset.UtcNow;
            game.UpdatedAt = DateTimeOffset.UtcNow;
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, game);
        }

        //PUT: /Games/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGame(int id,[FromBody] Game updateGame)
        {
            var game = await _context.Games.FindAsync(id);
            if(game == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            game.Id = updateGame.Id;
            game.Title = updateGame.Title;
            game.Description = updateGame.Description;
            game.ShortDescription = updateGame.ShortDescription;
            game.Price = updateGame.Price;
            game.DiscountPrice = updateGame.DiscountPrice;
            game.CoverImageUrl = updateGame.CoverImageUrl;
            game.TrailerUrl = updateGame.TrailerUrl;
            game.ScreenshotUrls = updateGame.ScreenshotUrls;
            game.Developer = updateGame.Developer;
            game.Publisher = updateGame.Publisher;
            game.ReleaseDate = updateGame.ReleaseDate;
            game.AgeRating = updateGame.AgeRating;
            game.Genres = updateGame.Genres;
            game.Platforms = updateGame.Platforms;
            game.Tags = updateGame.Tags;
            game.MinimumRequirements = updateGame.MinimumRequirements;
            game.RecommendedRequirements = updateGame.RecommendedRequirements;
            game.StockQuantity = updateGame.StockQuantity;
            game.IsAvailable = updateGame.IsAvailable;
            game.IsFeatured = updateGame.IsFeatured;
            game.UpdatedAt = DateTimeOffset.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        //DELETE: /Games/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
