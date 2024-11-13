
namespace BackendAssessment.Services
{
    public class CheckOutService : ICheckOutService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IProductRepository _productRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private PayStackApi Paystack { get; set; }

        public CheckOutService(IOrderRepository orderRepository, ITransactionRepository transactionRepository, IProductRepository productRepository, IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            var tokenValue = _configuration["Payment:PaystackSK"];
            Paystack = new PayStackApi(tokenValue);
            _orderRepository = orderRepository;
            _transactionRepository = transactionRepository;
            _productRepository = productRepository;

            _mapper = mapper;
        }
        public async Task<ResponseDto> ProcessCheckoutAsync(CheckOutRequest checkOutRequest, string email, CancellationToken cancellation = default)
        {
            try
            {
                // Validate the request and calculate total amount
                decimal totalAmount = 0;
                var products = new List<OrderItem>();

                foreach (var productQuantity in checkOutRequest.Products)
                {
                    var product = await _productRepository.GetByIdAsync(productQuantity.ProductId, cancellation);
                    if (product == null)
                        return ResponseDto.Failure(checkOutRequest, $"Product not found for this id {productQuantity.ProductId}");
                    if (product.Quantity < productQuantity.Quantity)
                        return ResponseDto.Failure(checkOutRequest, $"Product quantity is not enough for this id {productQuantity.ProductId}");

                    totalAmount += product.Price * productQuantity.Quantity;

                    products.Add(new OrderItem
                    {
                        ProductId = product.ProductId,
                        ProductName = product.Name,
                        Price = product.Price,
                        Quantity = productQuantity.Quantity
                    });

                }

                var order = new Order
                {
                    UserEmail = email, // Ideally, retrieve from the authenticated user context
                    OrderItems = products,
                    TotalAmount = totalAmount,
                    PaymentStatus = PaymentStatus.Pending,
                    PaymentStatusDesc = PaymentStatus.Pending.ToString(),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _orderRepository.AddAsync(order, cancellation);
                var orderDto = _mapper.Map<OrderDto>(order);


                //Generate a reference number
                var txRef = Guid.NewGuid().ToString("N");

                // Create a transaction
                var transaction = new Transaction
                {
                    Amount = totalAmount,
                    Currency = "NGN", // Adjust according to your needs
                    Status = PaymentStatus.Pending,
                    StatusDesc = PaymentStatus.Pending.ToString(),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    PaymentReference = txRef,
                    PaymentGateway = PaymentMethod.Paystack // or "Flutterwave"
                };
                await _transactionRepository.AddAsync(transaction, cancellation);

                var transactionDto = _mapper.Map<TransactionDto>(transaction);
                //Link the transaction to the order
                orderDto.TransactionId = transactionDto.Id;

                await _orderRepository.UpdateOrderAsync(orderDto, cancellation);

                var paymentResponse = InitiatePayment(transaction, email); // Pass verify URL
                if (string.IsNullOrEmpty(paymentResponse.AuthorizationUrl))
                {
                    return ResponseDto.Failure(paymentResponse, "Unable to complete checkout");
                }

                return ResponseDto.Success(paymentResponse, "Checkout successful. Proceed to confirm payment");
            }
            catch (ResourceNotFoundException ex)
            {
                return ResponseDto.Failure($"{ex.Message} for this order");

            }
            catch (Exception ex)
            {
                return ResponseDto.Failure($"An error occurred while processing the checkout: {ex.Message}", ex);
            }
        }

        protected virtual PaymentResponse InitiatePayment(Transaction transaction, string email)
        {

            TransactionInitializeRequest request = new()
            {
                AmountInKobo = (int)transaction.Amount * 100,
                Email = email,
                Reference = transaction.PaymentReference,
                Currency = "NGN"
            };

            TransactionInitializeResponse response = Paystack.Transactions.Initialize(request);
            if (response.Status)
            {
                // Return the PaymentResponse containing the authorization URL and payment reference
                return new PaymentResponse
                {
                    AuthorizationUrl = response.Data.AuthorizationUrl,
                    PaymentReference = response.Data.Reference
                };
            }
            else
            {
                return new PaymentResponse { AuthorizationUrl = string.Empty, PaymentReference = string.Empty };
            }
        }
    }
}
