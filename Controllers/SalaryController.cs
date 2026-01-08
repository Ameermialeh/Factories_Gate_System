using FactoriesGateSystem.DTOs.ExpenseDTOs;
using FactoriesGateSystem.DTOs.SalaryDTOs;
using FactoriesGateSystem.DTOs.VacationDTOs;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
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
                };
                return Ok(salaryDto);
            }
            catch { return StatusCode(500, "Internal server error"); }
        }


    }
}
