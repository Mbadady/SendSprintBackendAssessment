using AutoMapper;
using BackendAssessment.Exceptions;
using BackendAssessment.Interfaces;
using BackendAssessment.Interfaces.IRepositories;
using BackendAssessment.Models;
using BackendAssessment.Models.DTOs.Transaction;
using BackendAssessment.Util.Enums;
using Microsoft.EntityFrameworkCore;

namespace BackendAssessment.Repositories
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        private readonly IMapper _mapper;

        public TransactionRepository(IAppDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<TransactionDto> GetByReferenceAsync(string reference)
        {
            var transaction = await _context.Transactions.AsNoTracking().FirstOrDefaultAsync(t => t.PaymentReference == reference);

            return transaction == null ? throw new ResourceNotFoundException("Transaction not found") : _mapper.Map<TransactionDto>(transaction);
        }

        public async Task UpdateTransactionStatusAsync(TransactionDto transactionDto, PaymentStatus status, CancellationToken cancellationToken = default)
        {
            var transaction = await _context.Transactions.FindAsync([transactionDto.Id], cancellationToken) ?? throw new ResourceNotFoundException($"Entity with ID {transactionDto.Id} not found.");
            transaction.Status = status;
            transaction.UpdatedAt = DateTime.UtcNow;

            // Save changes to the database
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
