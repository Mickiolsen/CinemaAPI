using Microsoft.AspNetCore.Mvc;
using xxx.Repository.Interfaces;
using xxx.Repository.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace xxx.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly ICrudRepository<Movie> _context;

        public MovieController(ICrudRepository<Movie> context)
        {
            _context = context;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetAllMovies()
        {
            var movies = await _context.GetAll();
            return Ok(movies);
        }

        // GET: api/Movies/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var foundMovie = await _context.GetById(id);

            if (foundMovie == null)
            {
                return NotFound();
            }

            return Ok(foundMovie);
        }

        // POST: api/Movies
        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromBody] Movie movie)
        {
            if (movie == null)
            {
                return BadRequest("Movie is null.");
            }

            var newMovieCreated = await _context.Create(movie);
            if (newMovieCreated != null)
            {
                // Return the newly created movie with its URI
                return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movie);
            }

            return BadRequest("Failed to create movie.");
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMovieById(int id)
        {
            var isDeleted = await _context.DeleteById(id);

            if (isDeleted)
            {
                return NoContent(); 
            }

            return NotFound($"Movie with id {id} not found.");
        }
    }
}
