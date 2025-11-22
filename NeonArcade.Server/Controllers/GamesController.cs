using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeonArcade.Server.Models;
using NeonArcade.Server.Models.DTOs;
using NeonArcade.Server.Services.Interfaces;

namespace NeonArcade.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly ILogger<GamesController> _logger;

        public GamesController(IGameService gameService, ILogger<GamesController> logger)
        {
            _gameService = gameService;
            _logger = logger;
        }

        //GET: /Games (Public - No Authorization Required)
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PagedResult<Game>>> GetGames([FromQuery] GameQueryParameters parameters)
        {
            var result = await _gameService.GetGamesAsync(parameters);
            return Ok(result);
        }

        //GET: /Games/featured (Public - No Authorization Required)
        [HttpGet("featured")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Game>>> GetFeaturedGames([FromQuery] int count = 10)
        {
            var games = await _gameService.GetFeaturedGamesAsync(count);
            return Ok(games);
        }

        //GET: /Games/new-releases (Public - No Authorization Required)
        [HttpGet("new-releases")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Game>>> GetNewReleases([FromQuery] int count = 10)
        {
            var games = await _gameService.GetNewReleasesAsync(count);
            return Ok(games);
        }

        //GET: /Games/deals (Public - No Authorization Required)
        [HttpGet("deals")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Game>>> GetDiscountedGames()
        {
            var games = await _gameService.GetDiscountedGamesAsync();
            return Ok(games);
        }

        //GET: /Games/search (Public - No Authorization Required)
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Game>>> SearchGames([FromQuery] string searchTerm)
        {
            var games = await _gameService.SearchGamesAsync(searchTerm);
            return Ok(games);
        }

        //GET: /Games/genre/{genre} (Public - No Authorization Required)
        [HttpGet("genre/{genre}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Game>>> GetGamesByGenre(string genre)
        {
            var games = await _gameService.GetGamesByGenreAsync(genre);
            return Ok(games);
        }

        //GET: /Games/platform/{platform} (Public - No Authorization Required)
        [HttpGet("platform/{platform}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Game>>> GetGamesByPlatform(string platform)
        {
            var games = await _gameService.GetGamesByPlatformAsync(platform);
            return Ok(games);
        }

        //GET: /Games/{id} (Public - No Authorization Required)
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _gameService.GetGameByIdAsync(id);
            
            if (game == null)
            {
                return NotFound(new { message = $"Game with ID {id} not found" });
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

            var createdGame = await _gameService.CreateGameAsync(game);
            return CreatedAtAction(nameof(GetGame), new { id = createdGame.Id }, createdGame);
        }

        //PUT: /Games/{id} (Admin Only)
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateGame(int id, [FromBody] Game game)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedGame = await _gameService.UpdateGameAsync(id, game);
            return Ok(updatedGame);
        }

        //DELETE: /Games/{id} (Admin Only)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var result = await _gameService.DeleteGameAsync(id);
            
            if (!result)
            {
                return NotFound(new { message = $"Game with ID {id} not found" });
            }
            
            return NoContent();
        }

        //GET: /Games/{id}/stock (Public - No Authorization Required)
        [HttpGet("{id}/stock")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> CheckStock(int id, [FromQuery] int quantity = 1)
        {
            var inStock = await _gameService.IsGameInStockAsync(id, quantity);
            return Ok(new { gameId = id, quantity, inStock });
        }
    }
}
