using FactoriesGateSystem.DTOs;
using FactoriesGateSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FactoriesGateSystem.Repositories
{
    public class VacationRepo
    {
        private readonly AppDbContext _appDbContext;

        public VacationRepo (AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<VacationDTO>?> GetAllVacationAsync(Expression<Func<Vacation, bool>>? filter = null)
        {
            IQueryable<Vacation> query = _appDbContext.vacations;
            if (filter != null)
                query = query.Where(filter);

            return await query.Select(v => new VacationDTO
            {
                VacationId = v.VacationId,
                EmployeeId = v.EmployeeId,
                FromDate = v.FromDate,
                ToDate = v.ToDate,  
                VacationReason = v.VacationReason,
            }).ToListAsync();
        }

        public async Task<Vacation?> GetVacationByIdAsync(int id)
        {
            return await _appDbContext.vacations.Where(v => v.VacationId == id).FirstOrDefaultAsync();
        }
    }
}
