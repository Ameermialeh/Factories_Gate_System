using FactoriesGateSystem.DTOs;
using FactoriesGateSystem.DTOs.EmployeeDTOs;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "manager")]
    public class VacationController : Controller
    {
        private readonly VacationRepo _vacationRepo;

        public VacationController(VacationRepo vacationRepo)
        {
            _vacationRepo = vacationRepo;
        }

        [HttpGet]
        [ProducesResponseType(typeof(VacationDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllVacation()
        {
            try
            {
                var vacations = await _vacationRepo.GetAllVacationAsync();

                if(vacations == null) { return NotFound("Vacations not Found!"); }
                return Ok(vacations);
            }catch { return StatusCode(500, "Internal server error"); }
        }

    }
}
