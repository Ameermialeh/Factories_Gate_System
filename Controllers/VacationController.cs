using FactoriesGateSystem.Models.DTOs.VacationDTOs;
using FactoriesGateSystem.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "manager")]
    public class VacationController : Controller
    {

        private readonly IVacationService _vacationService;
        public VacationController(IVacationService vacationService)
        {
            _vacationService = vacationService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<VacationDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllVacation()
        {
            var vacations = await _vacationService.GetAllVacationAsync();
            return Ok(vacations);
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

            var vacation = await _vacationService.GetVacationByIdAsync(id);
            return Ok(vacation);
        }

        [HttpPost]
        [ProducesResponseType(typeof(VacationDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddVacation([FromBody] CreateVacationDTO dto)
        {
            if (dto.EmployeeId <= 0)
                return BadRequest("Invalid employee id.");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var vacation = await _vacationService.AddVacationAsync(dto);
            return Ok(vacation);
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
            var vacation = await _vacationService.UpdateVacationAsync(id, dto);
            return Ok(vacation);
        }

       

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteVacation(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid vacation id.");

            await _vacationService.DeleteVacationAsync(id);
            return Ok($"Vacation with {id} deleted Successfully");
        }
    }
}
