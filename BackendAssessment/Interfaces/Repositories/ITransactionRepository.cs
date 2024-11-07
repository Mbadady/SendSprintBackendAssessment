

namespace BackendAssessment.Interfaces.IRepositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task UpdateTransactionStatusAsync(TransactionDto transactionDto, PaymentStatus status, CancellationToken cancellationToken = default);
        Task<TransactionDto> GetByReferenceAsync(string reference);
    }
}
