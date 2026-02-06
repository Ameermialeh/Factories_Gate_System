using FactoriesGateSystem.Models.DTOs.EmployeeDTOs;
using FactoriesGateSystem.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "manager")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<EmployeeDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
 
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

            var employee = await _employeeService.GetEmployeeByIdAsync(id);

            return Ok(employee);
        }

        [HttpGet("{name:alpha}")]
        [ProducesResponseType(typeof(List<EmployeeDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetEmployeeName(string name)
        {
            var employees = await _employeeService.GetEmployeeNameAsync(name);
            return Ok(employees); 
        }

        [HttpPost]
        [ProducesResponseType(typeof(EmployeeDTO), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeDTO dto ) {

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var employee = await _employeeService.CreateEmployeeAsync(dto);
            return Ok(employee);
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

            var employee = await _employeeService.UpdateEmployeeAsync(id, dto);
            return Ok(employee);

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

            await _employeeService.DeleteEmployeeAsync(id);
            return Ok("Deleted Employee Successfuly");
        }
    }
}
