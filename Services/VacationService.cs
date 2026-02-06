using FactoriesGateSystem.Models.DTOs.VacationDTOs;
using FactoriesGateSystem.Repositories.Interfaces;
using FactoriesGateSystem.Services.ServiceInterfaces;

namespace FactoriesGateSystem.Services
{
    public class VacationService : IVacationService
    {
        private readonly IVacationRepo _vacationRepo;

        public VacationService(IVacationRepo vacationRepo)
        {
            _vacationRepo = vacationRepo;
        }
        public async Task<List<VacationDTO>> GetAllVacationAsync()
        {
            var vacations = await _vacationRepo.GetAllVacationAsync();
            return vacations;
        }
        public async Task<VacationDTO> GetVacationByIdAsync(int id)
        {
            var vacation = await _vacationRepo.GetVacationByIdAsync(id)
                ?? throw new BadHttpRequestException("Vacation Not Found", StatusCodes.Status404NotFound);

            var vacationDto = new VacationDTO
            {
                VacationId = vacation.VacationId,
                EmployeeId = vacation.EmployeeId,
                FromDate = vacation.FromDate,
                ToDate = vacation.ToDate,
                VacationReason = vacation.VacationReason,
            };
            return vacationDto;
        }
        public async Task<VacationDTO> AddVacationAsync(CreateVacationDTO dto)
        {
            var vacation = await _vacationRepo.AddVacationToEmployee(dto);
            return vacation;
        } 
        public async Task<VacationDTO> UpdateVacationAsync(int id, UpdateVacationDTO dto)
        {
            var vacation = await _vacationRepo.UpdateVacationAsync(id, dto)
                ?? throw new BadHttpRequestException("Vacation Not Found", StatusCodes.Status404NotFound);
            return vacation;
        }
        public async Task DeleteVacationAsync(int id)
        {
            var done = await _vacationRepo.DeleteVacationAsync(id);
            if (!done) { throw new BadHttpRequestException("Vacation Not Found", StatusCodes.Status404NotFound); }
        }
    }
}
