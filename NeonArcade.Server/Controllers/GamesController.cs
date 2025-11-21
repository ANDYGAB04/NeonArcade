using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeonArcade.Server.Data;
using NeonArcade.Server.Models;
using NeonArcade.Server.Models.DTOs;

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

        //GET: /Games (Public - No Authorization Required)
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PagedResult<Game>>> GetGames([FromQuery] GameQueryParameters parameters)
        {
            var query = _context.Games.AsQueryable();
            if(!string.IsNullOrEmpty(parameters.SearchTerm))
            {
                query = query.Where(g => g.Title.ToLower().Contains(parameters.SearchTerm.ToLower()) || g.Description.ToLower().Contains(parameters.SearchTerm.ToLower()));
            }
            if(parameters.MinPrice.HasValue)
            {
                query = query.Where(g => g.Price >= parameters.MinPrice.Value);
            }
            if(parameters.MaxPrice.HasValue)
            {
                query = query.Where(g => g.Price <= parameters.MaxPrice.Value);
            }
            if(parameters.IsAvailable.HasValue)
            {
                query = query.Where(g => g.IsAvailable == parameters.IsAvailable.Value);
            }
            if(parameters.IsFeatured.HasValue)
            {
                query = query.Where(g => g.IsFeatured == parameters.IsFeatured.Value);
            }
            // Sorting
            if(!string.IsNullOrEmpty(parameters.SortBy))
            {
                bool ascending = parameters.SortOrder?.ToLower() != "desc";
                query = parameters.SortBy.ToLower() switch
                {
                    "title" => ascending ? query.OrderBy(g => g.Title) : query.OrderByDescending(g => g.Title),
                    "price" => ascending ? query.OrderBy(g => g.Price) : query.OrderByDescending(g => g.Price),
                    "releasedate" => ascending ? query.OrderBy(g => g.ReleaseDate) : query.OrderByDescending(g => g.ReleaseDate),
                    _ => query
                };
            }
            else
            {
                query = query.OrderByDescending(g => g.CreatedAt);
            }
            var totalCount = await query.CountAsync();
            query = query.Skip((parameters.PageNumber - 1) * parameters.PageSize).Take(parameters.PageSize);
            var games = await query.ToListAsync();
            var result = new PagedResult<Game>(games, totalCount, parameters.PageNumber, parameters.PageSize);
            return Ok(result);
        }

        //GET: /Games/{id} (Public - No Authorization Required)
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetGame(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return Ok(game);
        }

        //POST: /Games (Admin Only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
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

        //PUT: /Games/{id} (Admin Only)
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
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

        //DELETE: /Games/{id} (Admin Only)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
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
