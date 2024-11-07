using AutoMapper;
using BackendAssessment.Exceptions;
using BackendAssessment.Interfaces.IRepositories;
using BackendAssessment.Interfaces.Repositories;
using BackendAssessment.Interfaces.Services;
using BackendAssessment.Models;
using BackendAssessment.Models.DTOs.Order;
using BackendAssessment.Models.DTOs.Payment;
using BackendAssessment.Util.Enums;
using System.Text.Json;

namespace BackendAssessment.Services
{
    public class WebhookService : IWebhookService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public WebhookService(ITransactionRepository transactionRepository, IOrderRepository orderRepository, IProductRepository productRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task HandleWebhookAsync(WebhookRequest webhookRequest)
        {
            var transactionReference = webhookRequest.Data.Reference;
            var transactionDto = await _transactionRepository.GetByReferenceAsync(transactionReference);
            var transaction = _mapper.Map<Transaction>(transactionDto);

            if (transaction == null)
            {
                throw new ResourceNotFoundException("Transaction not found.");
            }

            var order = await _orderRepository.FindByTransactionIdAsync(transaction.Id);
            if (order == null)
            {
                throw new ResourceNotFoundException("Order not found.");
            }

            var orderItems = JsonSerializer.Deserialize<List<OrderItem>>(order.OrderItemsJson);
            UpdateTransactionAndOrderStatus(webhookRequest.Event, order, transaction);

            if (order.PaymentStatus == PaymentStatus.APPROVED)
            {
                foreach (var orderItem in orderItems)
                {
                    await _productRepository.UpdateProductQuantity(orderItem.ProductId, orderItem.Quantity, CancellationToken.None);
                }
            }

            await _transactionRepository.UpdateAsync(transaction);
            await _orderRepository.UpdateAsync(order);
        }

        private static void UpdateTransactionAndOrderStatus(string webhookEvent, Order order, Transaction transaction)
        {
            switch (webhookEvent)
            {
                case "charge.success":
                    order.PaymentStatus = PaymentStatus.APPROVED;
                    order.PaymentStatusDesc = PaymentStatus.APPROVED.ToString();
                    transaction.Status = PaymentStatus.APPROVED;
                    transaction.StatusDesc = PaymentStatus.APPROVED.ToString();
                    break;
                case "charge.failed":
                    order.PaymentStatus = PaymentStatus.FAILED;
                    order.PaymentStatusDesc = PaymentStatus.FAILED.ToString();
                    transaction.Status = PaymentStatus.FAILED;
                    transaction.StatusDesc = PaymentStatus.FAILED.ToString();
                    break;
                default:
                    throw new ArgumentException("Invalid webhook event.");
            }
        }
    }
}
