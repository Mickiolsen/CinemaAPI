using Cinema.Repository.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xxx.Repository.Interfaces;

namespace Cinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        private readonly ICrudRepository<Seat> _context;

        public SeatController(ICrudRepository<Seat> context)
        {
            _context = context;
        }

        // GET: api/Seats
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Seat>>> GetAllSeats()
        {
            var seats = await _context.GetAll();
            return Ok(seats);
        }

        // GET: api/Seats/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetSeatById(int id)
        {
            var foundSeat = await _context.GetById(id);

            if (foundSeat == null)
            {
                return NotFound();
            }

            return Ok(foundSeat);
        }

        // POST: api/Seats
        [HttpPost]
        public async Task<IActionResult> CreateSeat([FromBody] Seat seat)
        {
            if (seat == null)
            {
                return BadRequest("Seat is null.");
            }

            var newSeatCreated = await _context.Create(seat);
            if (newSeatCreated != null)
            {
                // Return the newly created seat with its URI
                return CreatedAtAction(nameof(GetSeatById), new { id = seat.Id }, seat);
            }

            return BadRequest("Failed to create seat.");
        }

        // DELETE: api/Seats/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteSeatById(int id)
        {
            var isDeleted = await _context.DeleteById(id);

            if (isDeleted)
            {
                return NoContent();
            }

            return NotFound($"Seat with id {id} not found.");
        }
    }
}
