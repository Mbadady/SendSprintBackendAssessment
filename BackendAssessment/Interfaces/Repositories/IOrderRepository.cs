

namespace BackendAssessment.Interfaces.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task UpdateOrderAsync(OrderDto orderDto, CancellationToken cancellationToken = default);
        Task UpdateOrderPaymentStatusAsync(OrderDto orderDto, PaymentStatus status, CancellationToken cancellationToken = default);
        Task<Order> FindByTransactionIdAsync(int transactionId, CancellationToken cancellationToken = default);

    }
}
