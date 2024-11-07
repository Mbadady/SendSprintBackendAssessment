using BackendAssessment.Interfaces.IRepositories;
using BackendAssessment.Models;
using BackendAssessment.Models.DTOs.Order;
using BackendAssessment.Util.Enums;

namespace BackendAssessment.Interfaces.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task UpdateOrderAsync(OrderDto orderDto, CancellationToken cancellationToken = default);
        Task UpdateOrderPaymentStatusAsync(OrderDto orderDto, PaymentStatus status, CancellationToken cancellationToken = default);
        Task<Order> FindByTransactionIdAsync(int transactionId, CancellationToken cancellationToken = default);

    }
}
