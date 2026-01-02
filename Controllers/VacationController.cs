using FactoriesGateSystem.DTOs.EmployeeDTOs;
using FactoriesGateSystem.DTOs.VacationDTOs;
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

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(VacationDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetVacationById(int id)
        {
            try
            {
                var vacation = await _vacationRepo.GetVacationByIdAsync(id);
                if(vacation == null) { return NotFound($"No vacation with id = {id}. "); }

                var vacationDto = new VacationDTO
                {
                    VacationId = vacation.VacationId,
                    EmployeeId = vacation.EmployeeId,
                    FromDate = vacation.FromDate,
                    ToDate = vacation.ToDate,
                    VacationReason = vacation.VacationReason,
                };
                return Ok(vacation);
            }catch { return StatusCode(500, "Internal server error"); }
        }

        [HttpPost]
        [ProducesResponseType(typeof(VacationDTO), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddVacation(CreateVacationDTO dto)
        {
            try
            {
                var vacation = await _vacationRepo.AddVacationToEmployee(dto);
                return Ok(vacation);
            }
            catch { return StatusCode(500, "Internal server error"); }
        }

    }
}
