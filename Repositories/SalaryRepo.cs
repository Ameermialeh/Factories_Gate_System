using FactoriesGateSystem.DTOs.SalaryDTOs;
using FactoriesGateSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

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
                Month = s.Month 
            }).ToListAsync();
        }

        public async Task<Salary?> GetSalaryByIdAsync(int id)
        {
            return await _appDbContext.salaries.Where(s=>s.SalaryId == id).FirstOrDefaultAsync();
        }

        
    }
}
