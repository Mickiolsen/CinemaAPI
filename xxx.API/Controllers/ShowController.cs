using Cinema.Repository.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xxx.Repository.Interfaces;

namespace Cinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowController : ControllerBase
    {
        private readonly ICrudRepository<Show> _context;

        public ShowController(ICrudRepository<Show> context)
        {
            _context = context;
        }

        // GET: api/Shows
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Show>>> GetAllShows()
        {
            var shows = await _context.GetAll();
            return Ok(shows);
        }

        // GET: api/Shows/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetShowById(int id)
        {
            var foundShow = await _context.GetById(id);

            if (foundShow == null)
            {
                return NotFound();
            }

            return Ok(foundShow);
        }

        // POST: api/Shows
        [HttpPost]
        public async Task<IActionResult> CreateShow([FromBody] Show show)
        {
            if (show == null)
            {
                return BadRequest("Show is null.");
            }

            var newShowCreated = await _context.Create(show);
            if (newShowCreated != null)
            {
                // Return the newly created show with its URI
                return CreatedAtAction(nameof(GetShowById), new { id = show.Id }, show);
            }

            return BadRequest("Failed to create show.");
        }

        // DELETE: api/Shows/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteShowById(int id)
        {
            var isDeleted = await _context.DeleteById(id);

            if (isDeleted)
            {
                return NoContent();
            }

            return NotFound($"Show with id {id} not found.");
        }
    }
}
