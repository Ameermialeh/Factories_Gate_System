using FactoriesGateSystem.Models.DTOs.EmployeeDTOs;
using FactoriesGateSystem.Repositories.Interfaces;
using FactoriesGateSystem.Services.ServiceInterfaces;

namespace FactoriesGateSystem.Services
{
    public class EmployeeService : IEmployeeService 
    {
        private readonly IEmployeeRepo _employeeRepo;
        private readonly ICookieService _cookieService;

        public EmployeeService (IEmployeeRepo employeeRepo, ICookieService cookieService)
        {
            _employeeRepo = employeeRepo;
            _cookieService = cookieService;
        }

        public async Task<List<EmployeeDTO>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepo.GetEmployeesAsync();
            return employees;
        }

        public async Task<EmployeeDTO> GetEmployeeByIdAsync(int id)
        {
            var employee = await _employeeRepo.GetEmployeeByIdAsync(id) 
                ?? throw new BadHttpRequestException("Employee Not Found", StatusCodes.Status404NotFound);

            var employeeDto = new EmployeeDTO()
            {
                Id = employee.EmployeeId,
                Name = employee.Name,
                Phone = employee.Phone,
            };
            return employeeDto;
        }
        public async Task<List<EmployeeDTO>> GetEmployeeNameAsync(string name)
        {
            var employeeDto = await _employeeRepo.GetEmployeesAsync(e => e.Name.Contains(name));
            return employeeDto;
        }
        public async Task<EmployeeDTO> CreateEmployeeAsync(EmployeeDTO dto)
        {
            var factoryId = _cookieService.Get("FactoryId")
               ?? throw new BadHttpRequestException("Unauthorized User", StatusCodes.Status401Unauthorized);

            var employee = await _employeeRepo.CreateEmployeeAsync(dto, int.Parse(factoryId));
            return employee;
        }
        public async Task<EmployeeDTO> UpdateEmployeeAsync(int id, UpdateEmployeeDTO dto)
        {
            var employee = await _employeeRepo.UpdateEmployeeAsync(id, dto)
            ?? throw new BadHttpRequestException("Employee Not Found", StatusCodes.Status404NotFound);

            return employee;
        }
        public async Task DeleteEmployeeAsync(int id)
        {
            var done = await _employeeRepo.DeleteEmployeeAsync(id);
            if (!done) { throw new BadHttpRequestException("Employee Not Found", StatusCodes.Status404NotFound); }
        }
    }
}
