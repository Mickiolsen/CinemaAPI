using Cinema.Repository.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xxx.Repository.Interfaces;

namespace Cinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly ICrudRepository<Room> _context;

        public RoomController(ICrudRepository<Room> context)
        {
            _context = context;
        }

        // GET: api/Rooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetAllRooms()
        {
            var rooms = await _context.GetAll();
            return Ok(rooms);
        }

        // GET: api/Rooms/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetRoomById(int id)
        {
            var foundRoom = await _context.GetById(id);

            if (foundRoom == null)
            {
                return NotFound();
            }

            return Ok(foundRoom);
        }

        // POST: api/Rooms
        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] Room room)
        {
            if (room == null)
            {
                return BadRequest("Room is null.");
            }

            var newRoomCreated = await _context.Create(room);
            if (newRoomCreated != null)
            {
                // Return the newly created room with its URI
                return CreatedAtAction(nameof(GetRoomById), new { id = room.Id }, room);
            }

            return BadRequest("Failed to create room.");
        }

        // DELETE: api/Rooms/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteRoomById(int id)
        {
            var isDeleted = await _context.DeleteById(id);

            if (isDeleted)
            {
                return NoContent();
            }

            return NotFound($"Room with id {id} not found.");
        }
    }
}
