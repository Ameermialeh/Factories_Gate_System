using FactoriesGateSystem.DTOs.SalaryDTOs;
using FactoriesGateSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using static FactoriesGateSystem.DTOs.SalaryDTOs.UpdateSalaryDTO;

namespace FactoriesGateSystem.Repositories
{
    public class SalaryRepo
    {
        private readonly AppDbContext _appDbContext;

        public SalaryRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<SalaryDTO>?> GetAllSalariesAsync(Expression<Func<Salary, bool>>? filter = null)
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

        
        public async Task<SalaryDTO?> UpdateSalariesAsync(UpdateSalariesDTO dto)
        {
            var salary = await GetSalaryByIdAsync(dto.Id);
            if(salary == null) { return null; }

            salary.BaseSalary = dto.BaseSalary;
            salary.Bonus = dto.Bonus;
            salary.Deductions = dto.Deductions; 

            await _appDbContext.SaveChangesAsync();
            return new SalaryDTO { 
                Id = salary.SalaryId,
                BaseSalary = salary.BaseSalary,
                Bonus = salary.Deductions,
                Deductions= salary.Deductions,
                Date = salary.Date,
                EmployeeId  = salary.EmployeeId,
            };
        }

        public async Task<SalaryDTO?> UpdateSalariesDateAsync(UpdateDateSalaryDTO dto)
        {
            var salary = await GetSalaryByIdAsync(dto.Id);
            if (salary == null) { return null; }

            salary.Date = dto.Date;

            await _appDbContext.SaveChangesAsync();
            return new SalaryDTO
            {
                Id = salary.SalaryId,
                BaseSalary = salary.BaseSalary,
                Bonus = salary.Deductions,
                Deductions = salary.Deductions,
                Date = salary.Date,
                EmployeeId = salary.EmployeeId,
            };
        }
    }
}
