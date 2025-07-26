using DMS.BLL.Interfaces;
using DMS.DAL.Interfaces;
using DMS.Models.Entities;
using DMS.Models.DTOs;

namespace DMS.BLL.Services
{
    public class FeeService : IFeeService
    {
        private readonly IFeeRepository _feeRepository;
        private readonly IUserRepository _userRepository;

        public FeeService(IFeeRepository feeRepository, IUserRepository userRepository)
        {
            _feeRepository = feeRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<FeeResponseDTO>> GetAllAsync(string? type, int? month, int? year)
        {
            return await _feeRepository.GetAllWithFiltersAsync(type, month, year);
        }

        public async Task<FeeResponseDTO?> GetByIdAsync(int id)
        {
            return await _feeRepository.GetByIdWithDetailsAsync(id);
        }

        public async Task<FeeResponseDTO> CreateAsync(FeeCreateDTO feeDto)
        {
            var student = await _userRepository.GetByIdAsync(feeDto.StudentId);
            if (student == null)
            {
                throw new ArgumentException($"Student with id {feeDto.StudentId} does not exist.");
            }

            var fee = new Fee
            {
                StudentId = feeDto.StudentId,
                Type = feeDto.Type,
                Amount = feeDto.Amount,
                Status = feeDto.Status ?? "unpaid",
                CreatedAt = DateTime.Now,
                DueDate = feeDto.DueDate
            };

            await _feeRepository.CreateAsync(fee);

            return new FeeResponseDTO
            {
                Id = fee.Id,
                StudentId = fee.StudentId,
                StudentName = student.Name,
                Type = fee.Type,
                Amount = fee.Amount,
                Status = fee.Status,
                CreatedAt = fee.CreatedAt,
                DueDate = fee.DueDate,
                TotalPaidAmount = 0,
                RemainingAmount = fee.Amount,
                Transactions = new List<TransactionResponseDTO>()
            };
        }

        public async Task UpdateAsync(int id, FeeUpdateDTO feeDto)
        {
            var fee = await _feeRepository.GetByIdAsync(id);
            if (fee == null)
            {
                throw new ArgumentException("Fee not found");
            }

            fee.Type = feeDto.Type;
            fee.Amount = feeDto.Amount;
            fee.Status = feeDto.Status;
            fee.DueDate = feeDto.DueDate;

            await _feeRepository.UpdateAsync(fee);
        }

        public async Task DeleteAsync(int id)
        {
            var fee = await _feeRepository.GetByIdAsync(id);
            if (fee == null)
            {
                throw new ArgumentException("Fee not found");
            }

            await _feeRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<FeeResponseDTO>> GetByStudentAsync(int studentId)
        {
            return await _feeRepository.GetByStudentAsync(studentId);
        }

        public async Task<IEnumerable<FeeResponseDTO>> GetOverdueFeesAsync()
        {
            return await _feeRepository.GetOverdueFeesAsync();
        }
    }
} 