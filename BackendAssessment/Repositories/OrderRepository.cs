using BackendAssessment.Exceptions;
using BackendAssessment.Interfaces;
using BackendAssessment.Interfaces.Repositories;
using BackendAssessment.Models;
using BackendAssessment.Models.DTOs.Order;
using BackendAssessment.Util.Enums;
using Microsoft.EntityFrameworkCore;

namespace BackendAssessment.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(IAppDbContext context) : base(context)
        {
        }

        public async Task<Order> FindByTransactionIdAsync(int transactionId, CancellationToken cancellationToken = default)
        {
            var order = await _context.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.TransactionId == transactionId, cancellationToken);
            return order ?? throw new ResourceNotFoundException($"Order with transaction ID {transactionId} not found.");
        }

        public async Task UpdateOrderAsync(OrderDto orderDto, CancellationToken cancellationToken = default)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderDto.Id, cancellationToken: cancellationToken);
            if (order == null)
                throw new ResourceNotFoundException($"Order with ID {orderDto.Id} not found.");

            // Update necessary properties
            order.TransactionId = orderDto.TransactionId;
            order.UpdatedAt = orderDto.UpdatedAt;
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateOrderPaymentStatusAsync(OrderDto orderDto, PaymentStatus status, CancellationToken cancellationToken = default)
        {
            var order = await _context.Orders.FindAsync([orderDto.Id], cancellationToken) ?? throw new ResourceNotFoundException($"Entity with ID {orderDto.Id} not found.");
            order.PaymentStatus = status;

            // Save changes to the database
            _context.Orders.Update(order);
            await _context.SaveChangesAsync(cancellationToken);
        }


    }
}
