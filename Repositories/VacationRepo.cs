using FactoriesGateSystem.DTOs.VacationDTOs;
using FactoriesGateSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static FactoriesGateSystem.DTOs.VacationDTOs.UpdateVacationDTO;
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

        public async Task<VacationDTO> AddVacationToEmployee(CreateVacationDTO dto)
        {
            var Vacation = new Vacation
            {
                EmployeeId = dto.EmployeeId,
                FromDate = dto.FromDate,
                ToDate = dto.ToDate,
                VacationReason = dto.VacationReason!
            };

            await _appDbContext.vacations.AddAsync(Vacation);
            await _appDbContext.SaveChangesAsync();
            return new VacationDTO
            {
                VacationId = Vacation.VacationId,
                EmployeeId = dto.EmployeeId,
                FromDate = dto.FromDate,
                ToDate = dto.ToDate,
                VacationReason = dto.VacationReason,
            };
        }

        public async Task<VacationDTO?> UpdateVacationDateAsync(UpdateVacationDate dto)
        {
            var vacation =await _appDbContext.vacations.Where(v => v.VacationId == dto.VacationId).FirstOrDefaultAsync();
            if (vacation == null) { return null; }

            vacation.FromDate = dto.FromDate;
            vacation.ToDate = dto.ToDate;

            await _appDbContext.SaveChangesAsync();

            return new VacationDTO
            {
                VacationId = vacation.VacationId,   
                FromDate = vacation.FromDate,
                ToDate = vacation.ToDate,
                EmployeeId = vacation.EmployeeId,
                VacationReason = vacation.VacationReason,
            };
        }
        public async Task<VacationDTO?> UpdateVacationReasoneAsync(UpdateVacationReasone dto)
        {
            var vacation = await _appDbContext.vacations.Where(v => v.VacationId == dto.VacationId).FirstOrDefaultAsync();
            if (vacation == null) { return null; }

            vacation.VacationReason = dto.VacationReason!;
            await _appDbContext.SaveChangesAsync();

            return new VacationDTO
            {
                VacationId = vacation.VacationId,
                FromDate = vacation.FromDate,
                ToDate = vacation.ToDate,
                EmployeeId = vacation.EmployeeId,
                VacationReason = vacation.VacationReason,
            };
        }
    }
}
