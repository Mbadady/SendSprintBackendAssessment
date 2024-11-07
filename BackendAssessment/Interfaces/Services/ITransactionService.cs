namespace BackendAssessment.Interfaces.Services
{
    public interface ITransactionService
    {
        Task<ResponseDto> AddTransactionAsync(TransactionDto transaction, CancellationToken cancellationToken = default);
        Task<ResponseDto> UpdateTransactionStatusAsync(TransactionDto transactionDto, PaymentStatus status, CancellationToken cancellationToken = default);
    }
}
