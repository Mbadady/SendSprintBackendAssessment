namespace BackendAssessment.Services
{
    public class OrderService : IOrderService
    {
        private readonly IAppDbContext _context;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IAppDbContext context, IOrderRepository orderRepository, IMapper mapper)
        {
            _context = context;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        public async Task<ResponseDto> AddOrderAsync(OrderDto order, CancellationToken cancellationToken = default)
        {
            try
            {
                var entity = _mapper.Map<Order>(order);
                await _orderRepository.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                var orderDto = _mapper.Map<OrderDto>(entity);

                return ResponseDto.Success(orderDto, "Order created successfully");
            }
            catch (Exception ex)
            {
                return ResponseDto.Failure(order, $"An error occurred while creating the order: {ex.Message}", ex);
            }
        }

        public async Task<ResponseDto> DeleteOrderAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(id, cancellationToken);
                if (order == null)
                    return ResponseDto.Failure("Order not found.");

                await _orderRepository.RemoveAsync(order, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return ResponseDto.Success("Order deleted successfully");
            }
            catch (Exception ex)
            {
                return ResponseDto.Failure($"An error occurred while deleting the order: {ex.Message}", ex);
            }
        }

        public async Task<ResponseDto> GetAllOrdersAsync(string? searchTerm, int? skip, int? take, CancellationToken cancellationToken = default)
        {
            try
            {
                Expression<Func<Order, bool>> filter = o =>
                    string.IsNullOrEmpty(searchTerm) || o.UserEmail.ToLower().Contains(searchTerm.ToLower());

                var orders = await _orderRepository.GetAllAsync(skip, take, filter, cancellationToken);

                if (!orders.Any())
                {
                    return ResponseDto.Success("No order found");
                }

                var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
                return ResponseDto.Success(orderDtos, "Order(s) found successfully");
            }
            catch (Exception ex)
            {
                return ResponseDto.Failure($"An error occurred while fetching the orders: {ex.Message}", ex);
            }
        }

        public async Task<ResponseDto> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(id, cancellationToken);
                if (order == null)
                    return ResponseDto.Failure("Order not found.");

                var orderDto = _mapper.Map<OrderDto>(order);
                return ResponseDto.Success(orderDto, "Order found successfully");
            }
            catch (Exception ex)
            {
                return ResponseDto.Failure($"An error occurred while fetching the order: {ex.Message}", ex);
            }
        }

        public async Task<ResponseDto> UpdateOrderAsync(int id, OrderDto order, CancellationToken cancellationToken = default)
        {
            try
            {
                var existingOrder = await _orderRepository.GetByIdAsync(id, cancellationToken);
                if (existingOrder == null)
                    return ResponseDto.Failure(order, "Order not found.");

                _mapper.Map(order, existingOrder);

                await _orderRepository.UpdateAsync(existingOrder, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                var orderDto = _mapper.Map<OrderDto>(existingOrder);

                return ResponseDto.Success(orderDto, "Order updated successfully");
            }
            catch (Exception ex)
            {
                return ResponseDto.Failure(order, $"An error occurred while updating the order: {ex.Message}", ex);
            }
        }

        public async Task<ResponseDto> UpdateOrderTransactionIdAsync(OrderDto order, CancellationToken cancellationToken = default)
        {
            try
            {
                // Call the repository method to update the transaction status
                await _orderRepository.UpdateOrderAsync(order, cancellationToken);

                return ResponseDto.Success(order, "order transactionId updated successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseDto.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseDto.Failure($"An error occurred while updating the order: {ex.Message}", ex);
            }
        }
    }
}
