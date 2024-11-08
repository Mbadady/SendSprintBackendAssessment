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

            if (order.PaymentStatus == PaymentStatus.Approved)
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
                    order.PaymentStatus = PaymentStatus.Approved;
                    order.PaymentStatusDesc = PaymentStatus.Approved.ToString();
                    order.UpdatedAt = DateTime.UtcNow;
                    transaction.Status = PaymentStatus.Approved;
                    transaction.StatusDesc = PaymentStatus.Approved.ToString();
                    transaction.UpdatedAt = DateTime.UtcNow;
                    break;
                case "charge.failed":
                    order.PaymentStatus = PaymentStatus.Failed;
                    order.PaymentStatusDesc = PaymentStatus.Failed.ToString();
                    order.UpdatedAt = DateTime.Now;
                    transaction.Status = PaymentStatus.Failed;
                    transaction.StatusDesc = PaymentStatus.Failed.ToString();
                    transaction.UpdatedAt = DateTime.Now;
                    break;
                default:
                    throw new ArgumentException("Invalid webhook event.");
            }
        }
    }
}
