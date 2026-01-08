using FactoriesGateSystem.DTOs.ExpenseDTOs;
using FactoriesGateSystem.DTOs.SalaryDTOs;
using FactoriesGateSystem.DTOs.VacationDTOs;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FactoriesGateSystem.DTOs.SalaryDTOs.UpdateSalaryDTO;

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
        [ProducesResponseType(typeof(SalaryDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllSalaries()
        {
            try
            {
                var salaries = await _salaryRepo.GetAllSalariesAsync();

                if (salaries == null) { return NotFound("Salaries not Found!"); }
                return Ok(salaries);
            }
            catch { return StatusCode(500, "Internal server error"); }
        }


        [HttpGet("{id}")]
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

        [HttpGet("GetAllSalariesForEmployee/{employeeId}")]
        [ProducesResponseType(typeof(SalaryDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllSalariesForEmployee(int employeeId)
        {
            if (employeeId <= 0)
                return BadRequest("Invalid employee id.");
            try
            {
                var salary = await _salaryRepo.GetAllSalariesAsync(s => s.EmployeeId == employeeId);
                if (salary == null) { return NotFound($"No employee with id = {employeeId}. "); }
                return Ok(salary);
            }
            catch { return StatusCode(500, "Internal server error"); }
        }

        [HttpGet("GetAllSalariesInDateRange")]
        [ProducesResponseType(typeof(SalaryDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllSalariesInDateRange([FromBody] RangeDateSalaryDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid range.");
            try
            {
                var salary = await _salaryRepo.GetAllSalariesAsync(s => dto.FromDate <= s.Date && dto.ToDate >= s.Date);
                if (salary == null) { return NotFound($"No salaries in range {dto.FromDate} - {dto.ToDate}. "); }
                return Ok(salary);
            }
            catch { return StatusCode(500, "Internal server error"); }
        }

        [HttpPut("UpdateSalary")]
        [ProducesResponseType(typeof(SalaryDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateSalary([FromBody] UpdateSalariesDTO dto)
        {
            if (dto.Id <= 0 || dto.BaseSalary < 0 || dto.Bonus < 0 || dto.Deductions < 0)
                return BadRequest("Invalid Salaries data.");
            try
            {
                var salary = await _salaryRepo.UpdateSalariesAsync(dto);
                if (salary == null) { return NotFound($"No Salary with id = {dto.Id}. "); }
                return Ok(salary);
            }
            catch { return StatusCode(500, "Internal server error"); }
        }

        [HttpPut("UpdateDate")]
        [ProducesResponseType(typeof(SalaryDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateSalaryDate([FromBody] UpdateDateSalaryDTO dto)
        {
            if (dto.Id <= 0 || !ModelState.IsValid)
                return BadRequest("Invalid data.");
            try
            {
                var salary = await _salaryRepo.UpdateSalariesDateAsync(dto);
                if (salary == null) { return NotFound($"No Salary with id = {dto.Id}. "); }
                return Ok(salary);
            }
            catch { return StatusCode(500, "Internal server error"); }
        }


    }
}
