using FactoriesGateSystem.DTOs.ExpenseDTOs;
using FactoriesGateSystem.DTOs.SalaryDTOs;
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


    }
}
