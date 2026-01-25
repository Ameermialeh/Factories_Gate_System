using FactoriesGateSystem.Models;
using FactoriesGateSystem.Models.DTOs.VacationDTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
            var Vacation = new Vacation()
            {
                EmployeeId = dto.EmployeeId,
                FromDate = dto.FromDate!.Value,
                ToDate = dto.ToDate!.Value,
                VacationReason = dto.VacationReason!
            };

            await _appDbContext.vacations.AddAsync(Vacation);
            await _appDbContext.SaveChangesAsync();

            return new VacationDTO
            {
                VacationId = Vacation.VacationId,
                EmployeeId = dto.EmployeeId,
                FromDate = Vacation.FromDate,
                ToDate = Vacation.ToDate,
                VacationReason = Vacation.VacationReason,
            };
        }

        public async Task<VacationDTO?> UpdateVacationAsync(int id, UpdateVacationDTO dto)
        {
            var vacation =await _appDbContext.vacations.FirstOrDefaultAsync(v => v.VacationId == id);
            if (vacation == null) { return null; }

            if(dto.FromDate != null)
            {
                vacation.FromDate = dto.FromDate.Value;
            }

            if (dto.ToDate != null)
            {
                vacation.ToDate = dto.ToDate.Value;
            }

            if(dto.VacationReason != null)
            {
                vacation.VacationReason = dto.VacationReason;
            }

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
       
        public async Task<bool> DeleteVacationAsync(int id)
        {
            var vacation = await _appDbContext.vacations.Where(v => v.VacationId == id).FirstOrDefaultAsync();
            if (vacation == null) { return false; }
            
            _appDbContext.vacations.Remove(vacation);
            await _appDbContext.SaveChangesAsync(); 
            return true;
        }
    }
}
