using DMS.DAL.Interfaces;
using DMS.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMS.DAL.Repositories
{
    public class FacilityRepository : IFacilityRepository
    {
        private readonly DormManagementContext _context;

        public FacilityRepository(DormManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Facility>> GetAllAsync()
        {
            return await _context.Facilities.ToListAsync();
        }

        public async Task<IEnumerable<object>> GetAllWithDetailsAsync()
        {
            return await _context.Facilities
                .OrderBy(f => f.Name)
                .Select(f => new
                {
                    f.Id,
                    f.Name,
                    f.UnitPrice
                })
                .ToListAsync();
        }

        public async Task<Facility?> GetByIdAsync(int id)
        {
            return await _context.Facilities.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<object?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Facilities
                .Where(f => f.Id == id)
                .Select(f => new
                {
                    f.Id,
                    f.Name,
                    f.UnitPrice
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Facility> CreateAsync(Facility facility)
        {
            _context.Facilities.Add(facility);
            await _context.SaveChangesAsync();
            return facility;
        }

        public async Task<Facility> UpdateAsync(Facility facility)
        {
            _context.Facilities.Update(facility);
            await _context.SaveChangesAsync();
            return facility;
        }

        public async Task DeleteAsync(int id)
        {
            var facility = await _context.Facilities.FindAsync(id);
            if (facility != null)
            {
                _context.Facilities.Remove(facility);
                await _context.SaveChangesAsync();
            }
        }
    }
} 