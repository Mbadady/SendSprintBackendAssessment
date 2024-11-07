namespace BackendAssessment.Tests.Services.Orders
{
    public class UpdateOrderAsync : TestBase
    {
        [Test]
        public async Task UpdateOrderAsync_ValidInput_ReturnsSuccessResponseWithUpdatedOrder()
        {
            // Arrange
            var id = 1;
            var orderDto = new OrderDto
            {
                Id = id,
                UserEmail = "updated@test.com",
                PaymentStatus = PaymentStatus.Approved,
                TotalAmount = 150
            };
            var existingOrder = new Order
            {
                Id = id,
                UserEmail = "original@test.com",
                PaymentStatus = PaymentStatus.Pending,
                TotalAmount = 100
            };

            _orderRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(existingOrder);
            _mapperMock.Setup(m => m.Map(orderDto, existingOrder));
            _orderRepositoryMock.Setup(r => r.UpdateAsync(existingOrder, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _mapperMock.Setup(m => m.Map<OrderDto>(existingOrder)).Returns(orderDto);

            // Act
            var response = await _orderService.UpdateOrderAsync(id, orderDto);

            // Assert
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Result, Is.EqualTo(orderDto));
            Assert.That(response.Message, Is.EqualTo("Order updated successfully"));
        }

        [Test]
        public async Task UpdateOrderAsync_OrderNotFound_ReturnsFailureResponse()
        {
            // Arrange
            var id = 1;
            var orderDto = new OrderDto
            {
                Id = id,
                UserEmail = "nonexistent@test.com",
                PaymentStatus = PaymentStatus.Pending,
                TotalAmount = 100
            };
            _orderRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Order)null);

            // Act
            var response = await _orderService.UpdateOrderAsync(id, orderDto);

            // Assert
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Result, Is.Null);
            Assert.That(response.Message, Is.EqualTo("Order not found."));
        }

        [Test]
        public async Task UpdateOrderAsync_ExceptionThrown_ReturnsFailureResponse()
        {
            // Arrange
            var id = 1;
            var orderDto = new OrderDto
            {
                Id = id,
                UserEmail = "errorcase@test.com",
                PaymentStatus = PaymentStatus.Pending,
                TotalAmount = 100
            };
            var exceptionMessage = "An error occurred";
            _orderRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).Throws(new Exception(exceptionMessage));

            // Act
            var response = await _orderService.UpdateOrderAsync(id, orderDto);

            // Assert
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Result, Is.Null);
        }

    }
}
