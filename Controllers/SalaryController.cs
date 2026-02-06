using FactoriesGateSystem.Models.DTOs.SalaryDTOs;
using FactoriesGateSystem.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "manager")]
    public class SalaryController : Controller
    {

        private readonly ISalaryService _salaryService;

        public SalaryController(ISalaryService salaryService)
        {
            _salaryService = salaryService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<SalaryDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSalary([FromQuery] int? employeeId)
        {
            if(employeeId == null)
            {
                var salaries = await _salaryService.GetAllSalariesAsync();
                return Ok(salaries);
            }
            var filtered = await _salaryService.GetAllSalariesByEmployeeId(employeeId.Value);
            return Ok(filtered);
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

            var salary = await _salaryService.GetSalaryByIdAsync(id);
            return Ok(salary);
        }

        [HttpGet("DateRange")]
        [ProducesResponseType(typeof(List<SalaryDTO>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllSalariesInDateRange([FromQuery] int employeeId, [FromQuery] DateTime FromDate, [FromQuery] DateTime ToDate)
        {
            if(employeeId <= 0)
                return BadRequest("Employee Id invalid.");
            
            if (ToDate <= FromDate)
                return BadRequest("ToDate must be later than FromDate.");

            var salary = await _salaryService.GetAllSalariesInDateRangeAsync(employeeId , FromDate, ToDate);
            return Ok(salary);
        }

        [HttpPost]
        [ProducesResponseType(typeof(SalaryDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddSalary([FromBody] AddSalaryDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(dto.EmployeeId <= 0)
                return BadRequest("Employee Id invalid.");

            var salary = await _salaryService.AddSalaryAsync(dto);
            return Ok(salary);
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

            var salary = await _salaryService.UpdateSalaryAsync(id, dto);
            return Ok(salary);
        }

       
        [HttpDelete("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteSalary(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid salary id.");

            await _salaryService.DeleteSalaryAsync(id);
            return Ok($"Salary with {id} deleted Successfully");
        }
    }
}
