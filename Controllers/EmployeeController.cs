using FactoriesGateSystem.DTOs.EmployeeDTOs;
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
        [ProducesResponseType(typeof(EmployeeDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var employeeDto = await _employeeRepo.GetEmployeesAsync();
                if (employeeDto == null) { return NotFound("There is no employees Found."); }
                return Ok(employeeDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
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
                };
                return Ok(employeeDto);
            }
            catch(Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(EmployeeDTO), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeDTO dto ) {
            try
            {
                var employee = await _employeeRepo.CreateEmployeeAsync(dto);
                return Ok(employee);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(EmployeeDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeDTO dto)
        {
            if (id <= 0 || dto == null)
                return BadRequest("Invalid employee data.");
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

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(EmployeeDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid employee id.");
            try
            {
                var employee =await _employeeRepo.DeleteEmployeeAsync(id);
                if(employee == null) { return NotFound($"No Employee with id: {id}."); }

                var employeeDto = new EmployeeDTO()
                {
                    Id = id,
                    Name = employee.Name,  
                    Phone = employee.Phone,
                };
                return Ok(employeeDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
