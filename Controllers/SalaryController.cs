using FactoriesGateSystem.Models;
using FactoriesGateSystem.Models.DTOs.ProductDTOs;
using FactoriesGateSystem.Models.DTOs.SalaryDTOs;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "manager")]
    public class SalaryController : Controller
    {
        private readonly SalaryRepo _salaryRepo;

        public SalaryController(SalaryRepo salaryRepo)
        {
            _salaryRepo = salaryRepo;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<SalaryDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSalary([FromQuery] int?employeeId)
        {
            try
            { 
                if(employeeId == null)
                {
                    var salaries = await _salaryRepo.GetAllSalariesAsync();
                    return Ok(salaries);
                }
                var filtered = await _salaryRepo.GetAllSalariesAsync(s => s.EmployeeId == employeeId);
                if (!filtered.Any()) { return NotFound($"No employee with id = {employeeId}. "); }
                return Ok(filtered);
            }
            catch { return StatusCode(500, "Internal server error"); }
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(SalaryDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSalaryById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid salary id.");
            try
            {
                var salary = await _salaryRepo.GetSalaryByIdAsync(id);
                if (salary == null) { return NotFound($"No salary with id = {id}. "); }

                var salaryDto = new SalaryDTO
                {
                     Id = id,
                     BaseSalary = salary.BaseSalary,
                     Bonus = salary.Bonus,
                     Deductions = salary.Deductions,
                     EmployeeId = salary.EmployeeId,
                     Date = salary.Date
                };
                return Ok(salaryDto);
            }
            catch { return StatusCode(500, "Internal server error"); }
        }

        [HttpGet("DateRange")]
        [ProducesResponseType(typeof(SalaryDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllSalariesInDateRange([FromQuery] int employeeId, [FromQuery] DateTime FromDate, [FromQuery] DateTime ToDate)
        {
            if(employeeId <= 0)
                return BadRequest("Employee Id invalid.");
            
            if (ToDate <= FromDate)
                return BadRequest("ToDate must be later than FromDate.");
            try
            {
                var salary = await _salaryRepo.GetAllSalariesAsync(s => FromDate <= s.Date && ToDate >= s.Date && s.EmployeeId == employeeId);
                if (salary == null) { return NotFound($"No salaries in range {FromDate} - {ToDate}. "); }
                return Ok(salary);
            }
            catch { return StatusCode(500, "Internal server error"); }
        }

        [HttpPost]
        [ProducesResponseType(typeof(SalaryDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddSalary([FromBody] AddSalaryDTO dto)
        {
            if(dto.EmployeeId <= 0)
                return BadRequest("Employee Id invalid.");

            try
            {
                var salary = await _salaryRepo.AddSalaryForEmployeeAsync(dto);
                return Ok(salary);
            }
            catch { return StatusCode(500, "Internal server error"); }

        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(SalaryDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateSalary(int id, [FromBody] UpdateSalaryDTO dto)
        {
            if (id <= 0)
                return BadRequest("Invalid product id.");

            if (dto.BaseSalary == null && dto.Bonus == null && dto.Deductions == null && dto.Date == null)
                return BadRequest("At least one field (BaseSalary or Bonus or Deductions or Date) must be provided.");

            if (dto.BaseSalary < 0 || dto.Bonus < 0 || dto.Deductions < 0 )
                return BadRequest("BaseSalary and Bonus and Deductions cannot be negative.");

            try
            {
                var salary = await _salaryRepo.UpdateSalariesAsync(id, dto);
                if (salary == null) { return NotFound($"No Salary with id = {id}. "); }
                return Ok(salary);
            }
            catch { return StatusCode(500, "Internal server error"); }
        }

       
        [HttpDelete("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteSalary(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid salary id.");
            try
            {
                var done = await _salaryRepo.DeleteSalaryAsync(id);
                if (!done) { return NotFound($"No salary with id = {id}. "); }

                return Ok($"Salary with {id} deleted Successfully");

            }
            catch { return StatusCode(500, "Internal server error"); }
        }
    }
}
