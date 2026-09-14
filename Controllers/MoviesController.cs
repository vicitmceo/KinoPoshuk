using KinoPoshuk.BLL.DTO;
using KinoPoshuk.BLL.Interfaces;
using KinoPoshuk.DAL.Constants;
using Microsoft.AspNetCore.Mvc;

namespace KinoPoshuk.Controllers;

[ApiController]
[Route("api")]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return BadRequest(new { Error = "Не вказано назву фільму" });

        if (title.Length > OmdbConstants.MaxQueryLength)
            return BadRequest(new { Error = $"Назва не може перевищувати {OmdbConstants.MaxQueryLength} символів" });

        var movie = await _movieService.SearchAsync(title);
        if (movie is null) return NotFound(new { Error = "Фільм не знайдено" });

        return Ok(movie);
    }

    [HttpGet("favorites")]
    public async Task<IActionResult> GetFavorites()
    {
        var favorites = await _movieService.GetFavoritesAsync();
        return Ok(favorites);
    }

    [HttpPost("favorites")]
    public async Task<IActionResult> AddFavorite([FromBody] AddFavoriteRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.ImdbId))
            return BadRequest(new { Error = "ImdbId є обов'язковим" });

        var favorite = await _movieService.AddFavoriteAsync(request);
        return Ok(favorite);
    }

    [HttpDelete("favorites/{id:int}")]
    public async Task<IActionResult> RemoveFavorite(int id)
    {
        await _movieService.RemoveFavoriteAsync(id);
        return NoContent();
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory()
    {
        var history = await _movieService.GetHistoryAsync();
        return Ok(history);
    }
}
