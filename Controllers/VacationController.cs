using FactoriesGateSystem.Models.DTOs.SupplierDTOs;
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


        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(VacationDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateVacation(int id, [FromBody] UpdateVacationDTO dto)
        {
            if (id <= 0)
                return BadRequest("Invalid vacation id.");

            if (dto.VacationReason == null && dto.FromDate == null && dto.ToDate == null)
                return BadRequest("At least one field (VacationReason or FromDate or ToDate) must be provided.");

            if (dto.FromDate != null && dto.ToDate != null)
            {
                if (dto.ToDate.Value <= dto.FromDate.Value)
                    return BadRequest("ToDate must be later than FromDate.");
            }

            try
            {
                var vacation = await _vacationRepo.UpdateVacationAsync(id, dto);
                if (vacation == null) { return NotFound($"No vacation with id = {id}. "); }
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
