using FactoriesGateSystem.Models.DTOs.EmployeeDTOs;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "manager")]
    public class EmployeeController : Controller
    {
        private readonly EmployeeRepo _employeeRepo;
        public EmployeeController(EmployeeRepo employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<EmployeeDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var employeeDto = await _employeeRepo.GetEmployeesAsync();
                return Ok(employeeDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(EmployeeDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid employee id.");
            try
            {
                var employee = await _employeeRepo.GetEmployeeByIdAsync(id);
                if (employee == null) { return NotFound($"No employee with id = {id}."); }

                var employeeDto = new EmployeeDTO()
                {
                    Id = employee.EmployeeId,
                    Name = employee.Name,
                    Phone = employee.Phone,
                };
                return Ok(employeeDto);
            }
            catch(Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{name:alpha}")]
        [ProducesResponseType(typeof(List<EmployeeDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetEmployeeName(string name)
        {
            try
            {
                var employeeDto = await _employeeRepo.GetEmployeesAsync(e => e.Name.Contains(name));
                return Ok(employeeDto);
            }
            catch (Exception) { return StatusCode(500, "Internal server error"); }
        }

        [HttpPost]
        [ProducesResponseType(typeof(EmployeeDTO), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeDTO dto ) {
            try
            {
                var factoryId = Request.Cookies["FactoryId"];
                if (factoryId == null)
                    return Unauthorized();

                var employee = await _employeeRepo.CreateEmployeeAsync(dto,int.Parse(factoryId));
                return Ok(employee);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(EmployeeDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeDTO dto)
        {
            if (id <= 0)
                return BadRequest("Invalid employee id.");
            if (dto.Name == null && dto.Phone == null)
                return BadRequest("At least one field (Name or Phone) must be provided.");

            try
            {
                var employee =await _employeeRepo.UpdateEmployeeAsync(id, dto);   
                if(employee == null) { return NotFound($"No Employee with id: {id}."); }
                return Ok(employee);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid employee id.");
            try
            {
                var done = await _employeeRepo.DeleteEmployeeAsync(id);
                if(!done) { return NotFound($"No Employee with id: {id}."); }
                return Ok("Deleted Employee Successfuly");
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
