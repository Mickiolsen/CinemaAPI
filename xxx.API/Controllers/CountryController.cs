using Cinema.Repository.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xxx.Repository.Interfaces;

namespace Cinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICrudRepository<Country> _context;

        public CountryController(ICrudRepository<Country> context)
        {
            _context = context;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> GetAllCountries()
        {
            var countries = await _context.GetAll();
            return Ok(countries);
        }

        // GET: api/Countries/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCountryById(int id)
        {
            var foundCountry = await _context.GetById(id);

            if (foundCountry == null)
            {
                return NotFound();
            }

            return Ok(foundCountry);
        }

        // POST: api/Countries
        [HttpPost]
        public async Task<IActionResult> CreateCountry([FromBody] Country country)
        {
            if (country == null)
            {
                return BadRequest("country is null.");
            }

            var newCountryCreated = await _context.Create(country);
            if (newCountryCreated != null)
            {
                // Return the newly created country with its URI
                return CreatedAtAction(nameof(GetCountryById), new { id = country.Id }, country);
            }

            return BadRequest("Failed to create country.");
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCountryById(int id)
        {
            var isDeleted = await _context.DeleteById(id);

            if (isDeleted)
            {
                return NoContent();
            }

            return NotFound($"Country with id {id} not found.");
        }
    }

}
