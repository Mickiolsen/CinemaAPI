using Cinema.Repository.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xxx.Repository.Interfaces;

namespace Cinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ICrudRepository<Ticket> _context;

        public TicketController(ICrudRepository<Ticket> context)
        {
            _context = context;
        }

        // GET: api/Tickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetAllTickets()
        {
            var tickets = await _context.GetAll();
            return Ok(tickets);
        }

        // GET: api/Tickets/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTicketById(int id)
        {
            var foundTicket = await _context.GetById(id);

            if (foundTicket == null)
            {
                return NotFound();
            }

            return Ok(foundTicket);
        }

        // POST: api/Tickets
        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] Ticket ticket)
        {
            if (ticket == null)
            {
                return BadRequest("ticket is null.");
            }

            var newTicketCreated = await _context.Create(ticket);
            if (newTicketCreated != null)
            {
                // Return the newly created ticket with its URI
                return CreatedAtAction(nameof(GetTicketById), new { id = ticket.Id }, ticket);
            }

            return BadRequest("Failed to create ticket.");
        }

        // DELETE: api/Tickets/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTicketById(int id)
        {
            var isDeleted = await _context.DeleteById(id);

            if (isDeleted)
            {
                return NoContent();
            }

            return NotFound($"Ticket with id {id} not found.");
        }
    }

}
