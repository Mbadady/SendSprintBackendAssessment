namespace BackendAssessment.Tests.Services.Orders
{
    public class GetOrderByIdAsync : TestBase
    {
        [Test]
        public async Task GetOrderByIdAsync_Should_Return_SuccessResponse_WithOrder()
        {
            // Arrange
            var id = 1;
            var order = new Order
            {
                Id = id,
                UserEmail = "example@test.com",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PaymentStatus = PaymentStatus.Approved,
                TotalAmount = 100
            };
            var orderDto = new OrderDto
            {
                Id = id,
                UserEmail = "example@test.com",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PaymentStatus = PaymentStatus.Approved,
                TotalAmount = 100
            };
            _orderRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(order);
            _mapperMock.Setup(m => m.Map<OrderDto>(order)).Returns(orderDto);

            // Act
            var response = await _orderService.GetOrderByIdAsync(id);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Result, Is.EqualTo(orderDto));
            Assert.That(response.Message, Is.EqualTo("Order found successfully"));
        }

        [Test]
        public async Task GetOrderByIdAsync_Should_Return_FailureResponse_When_OrderNotFound()
        {
            // Arrange
            var id = 1;
            _orderRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Order)null);

            // Act
            var response = await _orderService.GetOrderByIdAsync(id);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Result, Is.Null);
            Assert.That(response.Message, Is.EqualTo("Order not found."));
        }

        [Test]
        public async Task GetOrderByIdAsync_Should_Return_FailureResponse_When_ExceptionThrown()
        {
            // Arrange
            var id = 1;
            var exceptionMessage = "An error occurred";
            _orderRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).Throws(new Exception(exceptionMessage));

            // Act
            var response = await _orderService.GetOrderByIdAsync(id);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Result, Is.Null);
            Assert.That(response.Message, Is.EqualTo($"An error occurred while fetching the order: {exceptionMessage}"));
        }

    }
}
