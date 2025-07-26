using DMS.DAL.Interfaces;
using DMS.Models.Entities;
using DMS.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DMS.DAL.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        private readonly DormManagementContext _context;

        public RequestRepository(DormManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Request>> GetAllAsync()
        {
            return await _context.Requests.ToListAsync();
        }

        public async Task<IEnumerable<RequestResponseDTO>> GetAllWithDetailsAsync()
        {
            return await _context.Requests
                .Include(r => r.Student)
                .Include(r => r.Manager)
                .Select(r => new RequestResponseDTO
                {
                    Id = r.Id,
                    StudentId = r.StudentId,
                    StudentName = r.Student.Name,
                    ManagerId = r.ManagerId,
                    ManagerName = r.Manager != null ? r.Manager.Name : null,
                    Type = r.Type,
                    Description = r.Description,
                    Status = r.Status,
                    CreatedAt = r.CreatedAt
                })
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Request?> GetByIdAsync(int id)
        {
            return await _context.Requests.FindAsync(id);
        }

        public async Task<RequestResponseDTO?> GetByIdWithDetailsAsync(int id)
        {
            var request = await _context.Requests
                .Include(r => r.Student)
                .Include(r => r.Manager)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
                return null;

            return new RequestResponseDTO
            {
                Id = request.Id,
                StudentId = request.StudentId,
                StudentName = request.Student.Name,
                ManagerId = request.ManagerId,
                ManagerName = request.Manager != null ? request.Manager.Name : null,
                Type = request.Type,
                Description = request.Description,
                Status = request.Status,
                CreatedAt = request.CreatedAt
            };
        }

        public async Task<Request> CreateAsync(Request request)
        {
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task<Request> UpdateAsync(Request request)
        {
            _context.Requests.Update(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task DeleteAsync(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request != null)
            {
                _context.Requests.Remove(request);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<RequestResponseDTO>> GetByUserIdAsync(int userId)
        {
            return await _context.Requests
                .Include(r => r.Student)
                .Include(r => r.Manager)
                .Where(r => r.StudentId == userId)
                .Select(r => new RequestResponseDTO
                {
                    Id = r.Id,
                    StudentId = r.StudentId,
                    StudentName = r.Student.Name,
                    ManagerId = r.ManagerId,
                    ManagerName = r.Manager != null ? r.Manager.Name : null,
                    Type = r.Type,
                    Description = r.Description,
                    Status = r.Status,
                    CreatedAt = r.CreatedAt
                })
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<RequestResponseDTO>> GetByStatusAsync(string status)
        {
            return await _context.Requests
                .Include(r => r.Student)
                .Include(r => r.Manager)
                .Where(r => r.Status == status)
                .Select(r => new RequestResponseDTO
                {
                    Id = r.Id,
                    StudentId = r.StudentId,
                    StudentName = r.Student.Name,
                    ManagerId = r.ManagerId,
                    ManagerName = r.Manager != null ? r.Manager.Name : null,
                    Type = r.Type,
                    Description = r.Description,
                    Status = r.Status,
                    CreatedAt = r.CreatedAt
                })
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> UserExistsAsync(int userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }
    }
} 