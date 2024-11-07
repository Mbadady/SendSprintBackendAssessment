using BackendAssessment.Models.DTOs.Product;
using BackendAssessment.Models.DTOs;
using BackendAssessment.Models.DTOs.Transaction;
using BackendAssessment.Util.Enums;

namespace BackendAssessment.Interfaces.Services
{
    public interface ITransactionService
    {
        Task<ResponseDto> AddTransactionAsync(TransactionDto transaction, CancellationToken cancellationToken = default);
        Task<ResponseDto> UpdateTransactionStatusAsync(TransactionDto transactionDto, PaymentStatus status, CancellationToken cancellationToken = default);
    }
}
