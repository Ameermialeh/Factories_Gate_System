using FactoriesGateSystem.DTOs.EmployeeDTOs;
using FactoriesGateSystem.Models.DTOs.VacationDTOs;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FactoriesGateSystem.Models.DTOs.VacationDTOs.UpdateVacationDTO;

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
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetVacationById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid vacation id.");
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
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddVacation([FromBody] CreateVacationDTO dto)
        {
            if (dto.EmployeeId <= 0 || String.IsNullOrWhiteSpace(dto.VacationReason))
                return BadRequest("Invalid data.");

            try
            {
                var vacation = await _vacationRepo.AddVacationToEmployee(dto);
                return Ok(vacation);
            }
            catch { return StatusCode(500, "Internal server error"); }
        }


        [HttpPut("UpdateVacationDate")]
        [ProducesResponseType(typeof(VacationDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateVacationDate([FromBody] UpdateVacationDate dto)
        {
            if (dto.VacationId <= 0)
                return BadRequest("Invalid vacation id.");
            try
            {
                var vacation = await _vacationRepo.UpdateVacationDateAsync(dto);
                if (vacation == null) { return NotFound($"No vacation with id = {dto.VacationId}. "); }
                return Ok(vacation);
            }
            catch { return StatusCode(500, "Internal server error"); }
        }

        [HttpPut("UpdateVacationReasone")]
        [ProducesResponseType(typeof(VacationDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateVacationReasone([FromBody] UpdateVacationReasone dto)
        {
            if (dto.VacationId <= 0 || String.IsNullOrWhiteSpace(dto.VacationReason))
                return BadRequest("Invalid data.");
            try
            {
                var vacation = await _vacationRepo.UpdateVacationReasoneAsync(dto);
                if (vacation == null) { return NotFound($"No vacation with id = {dto.VacationId}. "); }
                return Ok(vacation);
            }
            catch { return StatusCode(500, "Internal server error"); }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteVacation(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid vacation id.");
            try
            {
                var done = await _vacationRepo.DeleteVacationAsync(id);
                if (!done) { return NotFound($"No vacation with id = {id}. "); }
                return Ok($"Vacation with {id} deleted Successfully");

            }catch { return StatusCode(500, "Internal server error"); }
        }
    }
}
