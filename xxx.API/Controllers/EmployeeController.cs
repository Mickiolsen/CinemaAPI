using Cinema.Repository.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xxx.Repository.Interfaces;

namespace Cinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ICrudRepository<Employee> _context;

        public EmployeeController(ICrudRepository<Employee> context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployees()
        {
            var employees = await _context.GetAll();
            return Ok(employees);
        }

        // GET: api/Employees/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var foundEmployee = await _context.GetById(id);

            if (foundEmployee == null)
            {
                return NotFound();
            }

            return Ok(foundEmployee);
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest("employee is null.");
            }

            var newEmployeeCreated = await _context.Create(employee);
            if (newEmployeeCreated != null)
            {
                // Return the newly created employee with its URI
                return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);
            }

            return BadRequest("Failed to create employee.");
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEmployeeById(int id)
        {
            var isDeleted = await _context.DeleteById(id);

            if (isDeleted)
            {
                return NoContent();
            }

            return NotFound($"Employee with id {id} not found.");
        }
    }

}
