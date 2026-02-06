using FactoriesGateSystem.Models.DTOs.SalaryDTOs;
using FactoriesGateSystem.Repositories.Interfaces;
using FactoriesGateSystem.Services.ServiceInterfaces;

namespace FactoriesGateSystem.Services
{
    public class SalaryService : ISalaryService
    {
        private readonly ISalaryRepo _salaryRepo;

        public SalaryService(ISalaryRepo salaryRepo)
        {
            _salaryRepo = salaryRepo;
        }
        public async Task<List<SalaryDTO>> GetAllSalariesAsync()
        {
            var salaries = await _salaryRepo.GetAllSalariesAsync();
            return salaries;
        }
        public async Task<List<SalaryDTO>> GetAllSalariesByEmployeeId(int employeeId)
        {
            var filtered = await _salaryRepo.GetAllSalariesAsync(s => s.EmployeeId == employeeId);
            if (filtered.Count == 0) { throw new BadHttpRequestException("Employee Not Found", StatusCodes.Status404NotFound); }
            return filtered;
        }
        public async Task<SalaryDTO> GetSalaryByIdAsync(int id)
        {
            var salary = await _salaryRepo.GetSalaryByIdAsync(id)
                ?? throw new BadHttpRequestException($"Salary with {id} Not Found", StatusCodes.Status404NotFound);

            var salaryDto = new SalaryDTO
            {
                Id = id,
                BaseSalary = salary.BaseSalary,
                Bonus = salary.Bonus,
                Deductions = salary.Deductions,
                EmployeeId = salary.EmployeeId,
                Date = salary.Date
            };
            return salaryDto;
        }
        public async Task<List<SalaryDTO>> GetAllSalariesInDateRangeAsync(int employeeId, DateTime FromDate, DateTime ToDate)
        {

            var salary = await _salaryRepo.GetAllSalariesAsync(s => FromDate <= s.Date && ToDate >= s.Date && s.EmployeeId == employeeId)
                 ?? throw new BadHttpRequestException($"No salaries in range {FromDate} - {ToDate}. ", StatusCodes.Status404NotFound);

            return salary;
        }
        public async Task<SalaryDTO> AddSalaryAsync(AddSalaryDTO dto)
        {
            var salary = await _salaryRepo.AddSalaryForEmployeeAsync(dto);
            return salary;
        }
        public async Task<SalaryDTO> UpdateSalaryAsync(int id, UpdateSalaryDTO dto)
        {
            var salary = await _salaryRepo.UpdateSalariesAsync(id, dto)
                 ?? throw new BadHttpRequestException($"No Salary with id = {id}. ", StatusCodes.Status404NotFound);
            return salary;
        }
        public async Task DeleteSalaryAsync(int id)
        {
            var done = await _salaryRepo.DeleteSalaryAsync(id);
            if (!done) { throw new BadHttpRequestException($"No Salary with id = {id}. ", StatusCodes.Status404NotFound); }
        }
    }
}
