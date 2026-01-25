using FactoriesGateSystem.Models;
using FactoriesGateSystem.Models.DTOs.EmployeeDTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FactoriesGateSystem.Repositories
{
    public class EmployeeRepo
    {
        private readonly AppDbContext _appDbContext;

        public EmployeeRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<EmployeeDTO>> GetEmployeesAsync(Expression<Func<Employee,bool>>? filter = null)
        {
            IQueryable<Employee> query = _appDbContext.employees;
            if (filter != null)
                query = query.Where(filter);

            return await query.Select(e=>new EmployeeDTO
            {
                Id = e.EmployeeId,
                Name = e.Name,
                Phone = e.Phone,
            }).ToListAsync();
        }
       
        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _appDbContext.employees.FirstOrDefaultAsync(e => e.EmployeeId == id);
        }


       public async Task<EmployeeDTO> CreateEmployeeAsync(EmployeeDTO employeeDto, int factoryId)
       {
            var employee = new Employee()
            {
                Name = employeeDto.Name!,
                Phone = employeeDto.Phone!,
                FactoryId = factoryId
            };
            await _appDbContext.employees.AddAsync(employee);
            await _appDbContext.SaveChangesAsync();
            employeeDto.Id = employee.EmployeeId;
            return employeeDto;
       }

        public async Task<EmployeeDTO?> UpdateEmployeeAsync(int id,UpdateEmployeeDTO dto)
        {
            var employee = await _appDbContext.employees.FindAsync(id);
            if (employee == null) return null;

            if(dto.Name != null)
            {
                employee.Name = dto.Name;
            }
            if (dto.Phone != null)
            {
                employee.Phone = dto.Phone;
            }
          
            await _appDbContext.SaveChangesAsync();
            var employeeDto = new EmployeeDTO()
            {
                Id = id,
                Name = employee.Name,
                Phone = employee.Phone,
            };
            return employeeDto;
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await _appDbContext.employees.FindAsync(id);
            if (employee == null) return false;

            var vacations = await _appDbContext.vacations.Where(v => v.EmployeeId == id).ToListAsync();

            if (vacations.Any())
                 _appDbContext.vacations.RemoveRange(vacations);
          
            _appDbContext.employees.Remove(employee);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
