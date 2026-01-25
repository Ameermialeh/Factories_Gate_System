using FactoriesGateSystem.Models;
using FactoriesGateSystem.Models.DTOs.SalaryDTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static FactoriesGateSystem.Models.DTOs.SalaryDTOs.UpdateSalaryDTO;

namespace FactoriesGateSystem.Repositories
{
    public class SalaryRepo
    {
        private readonly AppDbContext _appDbContext;

        public SalaryRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<SalaryDTO>> GetAllSalariesAsync(Expression<Func<Salary, bool>>? filter = null)
        {
            IQueryable<Salary> query = _appDbContext.salaries;
            if (filter != null)
                query = query.Where(filter);

            return await query.Select(s => new SalaryDTO
            {
                Id = s.SalaryId,
                BaseSalary = s.BaseSalary,
                Bonus = s.Bonus,
                Deductions = s.Deductions,
                EmployeeId = s.EmployeeId,
                Date = s.Date,
            }).ToListAsync();
        }

        public async Task<Salary?> GetSalaryByIdAsync(int id)
        {
            return await _appDbContext.salaries.Where(s=>s.SalaryId == id).FirstOrDefaultAsync();
        }

        
        public async Task<SalaryDTO> AddSalaryForEmployeeAsync(AddSalaryDTO dto)
        {
            Salary salary = new Salary
            {
                EmployeeId = dto.EmployeeId,
                BaseSalary = dto.BaseSalary,
                Bonus = dto.Bonus,
                Deductions = dto.Deductions,
                Date = dto.Date,
            };

            await _appDbContext.salaries.AddAsync(salary);
            await _appDbContext.SaveChangesAsync();

            return new SalaryDTO 
            {
                Id = salary.SalaryId,
                BaseSalary = salary.BaseSalary,
                Bonus = salary.Bonus,
                Date = dto.Date,
                Deductions = dto.Deductions,
                EmployeeId= dto.EmployeeId,
            };
        }

        public async Task<SalaryDTO?> UpdateSalariesAsync(int id, UpdateSalaryDTO dto)
        {
            var salary = await GetSalaryByIdAsync(id);
            if(salary == null) { return null; }

            if(dto.BaseSalary != null)
            {
                salary.BaseSalary = dto.BaseSalary.Value;
            }

            if(dto.Bonus != null)
            {
                salary.Bonus = dto.Bonus.Value;
            }

            if(dto.Deductions != null)
            {
                salary.Deductions = dto.Deductions.Value;
            }

            if(dto.Date != null)
            {
                salary.Date = dto.Date.Value;
            }
            await _appDbContext.SaveChangesAsync();
            return new SalaryDTO { 
                Id = id,
                BaseSalary = salary.BaseSalary,
                Bonus = salary.Deductions,
                Deductions= salary.Deductions,
                Date = salary.Date,
                EmployeeId  = salary.EmployeeId,
            };
        }
        public async Task<bool> DeleteSalaryAsync(int id)
        {
            var salary = await GetSalaryByIdAsync(id);
            if (salary == null) { return false; }

            _appDbContext.salaries.Remove(salary);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
