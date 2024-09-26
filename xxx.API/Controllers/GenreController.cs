using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xxx.Repository.Interfaces;
using xxx.Repository.Models;

namespace Cinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly ICrudRepository<Genre> _context;

        public GenreController(ICrudRepository<Genre> context)
        {
            _context = context;
        }

        // GET: api/Genres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genre>>> GetAllGenres()
        {
            var genres = await _context.GetAll();
            return Ok(genres);
        }

        // GET: api/Genres/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetGenreById(int id)
        {
            var foundGenre = await _context.GetById(id);

            if (foundGenre == null)
            {
                return NotFound();
            }

            return Ok(foundGenre);
        }

        // POST: api/Genres
        [HttpPost]
        public async Task<IActionResult> CreateGenre([FromBody] Genre genre)
        {
            if (genre == null)
            {
                return BadRequest("genre is null.");
            }

            var newGenreCreated = await _context.Create(genre);
            if (newGenreCreated != null)
            {
                // Return the newly created genre with its URI
                return CreatedAtAction(nameof(GetGenreById), new { id = genre.Id }, genre);
            }

            return BadRequest("Failed to create genre.");
        }

        // DELETE: api/Genres/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteGenreById(int id)
        {
            var isDeleted = await _context.DeleteById(id);

            if (isDeleted)
            {
                return NoContent();
            }

            return NotFound($"Genre with id {id} not found.");
        }
    }
}
