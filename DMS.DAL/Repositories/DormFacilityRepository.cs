using DMS.DAL.Interfaces;
using DMS.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMS.DAL.Repositories
{
    public class DormFacilityRepository : IDormFacilityRepository
    {
        private readonly DormManagementContext _context;

        public DormFacilityRepository(DormManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DormFacility>> GetAllAsync()
        {
            return await _context.DormFacilities.ToListAsync();
        }

        public async Task<IEnumerable<object>> GetAllWithDetailsAsync()
        {
            return await _context.DormFacilities
                .Select(df => new {
                    df.Id,
                    df.DormitoryId,
                    DormitoryName = df.Dormitory.Name,
                    df.FacilityId,
                    FacilityName = df.Facility.Name,
                    df.Quantity
                })
                .ToListAsync();
        }

        public async Task<DormFacility?> GetByIdAsync(int id)
        {
            return await _context.DormFacilities.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<DormFacility> CreateAsync(DormFacility dormFacility)
        {
            _context.DormFacilities.Add(dormFacility);
            await _context.SaveChangesAsync();
            return dormFacility;
        }

        public async Task<DormFacility> UpdateAsync(DormFacility dormFacility)
        {
            _context.DormFacilities.Update(dormFacility);
            await _context.SaveChangesAsync();
            return dormFacility;
        }

        public async Task DeleteAsync(int id)
        {
            var df = await _context.DormFacilities.FindAsync(id);
            if (df != null)
            {
                _context.DormFacilities.Remove(df);
                await _context.SaveChangesAsync();
            }
        }
    }
} 