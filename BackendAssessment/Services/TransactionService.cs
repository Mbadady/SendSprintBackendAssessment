using AutoMapper;
using BackendAssessment.Exceptions;
using BackendAssessment.Interfaces;
using BackendAssessment.Interfaces.IRepositories;
using BackendAssessment.Interfaces.Services;
using BackendAssessment.Models;
using BackendAssessment.Models.DTOs;
using BackendAssessment.Models.DTOs.Transaction;
using BackendAssessment.Util.Enums;

namespace BackendAssessment.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAppDbContext _context;

        public TransactionService(IMapper mapper, ITransactionRepository transactionRepository, IAppDbContext dbcontext)
        {
            _mapper = mapper;
            _transactionRepository = transactionRepository;
            _context = dbcontext;
        }
        public async Task<ResponseDto> AddTransactionAsync(TransactionDto transaction, CancellationToken cancellationToken = default)
        {
            try
            {
                var entity = _mapper.Map<Transaction>(transaction);
                await _transactionRepository.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                var transactionDto = _mapper.Map<TransactionDto>(entity);

                return ResponseDto.Success(transactionDto, "Transaction created successfully");
            }
            catch (Exception ex)
            {
                return ResponseDto.Failure(transaction, $"An error occurred while creating the transaction: {ex.Message}", ex);
            }
        }

        public async Task<ResponseDto> UpdateTransactionStatusAsync(TransactionDto transactionDto, PaymentStatus status, CancellationToken cancellationToken = default)
        {
            try
            {
                // Call the repository method to update the transaction status
                await _transactionRepository.UpdateTransactionStatusAsync(transactionDto, status, cancellationToken);

                return ResponseDto.Success(transactionDto, "Transaction status updated successfully.");
            }
            catch (ResourceNotFoundException ex)
            {
                return ResponseDto.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseDto.Failure($"An error occurred while updating the transaction: {ex.Message}", ex);
            }

        }
    }
}
