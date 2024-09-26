using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xxx.Repository.Interfaces;
using xxx.Repository.Models;

namespace Cinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorController : ControllerBase
    {
        private readonly ICrudRepository<Actor> _context;

        public ActorController(ICrudRepository<Actor> context)
        {
            _context = context;
        }

        // GET: api/Actors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Actor>>> GetAllActors()
        {
            var actors = await _context.GetAll();
            return Ok(actors);
        }

        // GET: api/Actors/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetActorById(int id)
        {
            var foundActor = await _context.GetById(id);

            if (foundActor == null)
            {
                return NotFound();
            }

            return Ok(foundActor);
        }

        // POST: api/Actors
        [HttpPost]
        public async Task<IActionResult> CreateActor([FromBody] Actor actor)
        {
            if (actor == null)
            {
                return BadRequest("actor is null.");
            }

            var newActorCreated = await _context.Create(actor);
            if (newActorCreated != null)
            {
                // Return the newly created actor with its URI
                return CreatedAtAction(nameof(GetActorById), new { id = actor.Id }, actor);
            }

            return BadRequest("Failed to create actor.");
        }

        // DELETE: api/Actors/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteActorById(int id)
        {
            var isDeleted = await _context.DeleteById(id);

            if (isDeleted)
            {
                return NoContent();
            }

            return NotFound($"Actor with id {id} not found.");
        }
    }

}
