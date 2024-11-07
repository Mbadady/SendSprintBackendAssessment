namespace BackendAssessment.Tests.Services.Orders
{
    public class DeleteOrderAsyncTests : TestBase
    {
        [Test]
        public async Task DeleteOrderAsync_Should_Return_SuccessResponse()
        {
            // Arrange
            var id = 1;
            var orderDto = new OrderDto
            {
                Id = id,
                TransactionId = 10,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PaymentStatus = PaymentStatus.Approved,
                TotalAmount = 10,
                UserEmail = "example@test.com"
            };

            var order = new Order
            {
                Id = orderDto.Id,
                TransactionId = orderDto.TransactionId,
                CreatedAt = orderDto.CreatedAt,
                UpdatedAt = orderDto.UpdatedAt,
                PaymentStatus = PaymentStatus.Approved,
                TotalAmount = orderDto.TotalAmount,
                UserEmail = orderDto.UserEmail
            };

            _orderRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(order);
            _orderRepositoryMock.Setup(r => r.RemoveAsync(order, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var response = await _orderService.DeleteOrderAsync(id);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Message, Is.EqualTo("Order deleted successfully"));
        }

        [Test]
        public async Task DeleteOrderAsync_Should_Return_FailureResponse_When_OrderNotFound()
        {
            // Arrange
            var id = 1;
            _orderRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Order)null);

            // Act
            var response = await _orderService.DeleteOrderAsync(id);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(response.IsSuccess, Is.False);
                Assert.That(response.Message, Is.EqualTo("Order not found."));
            });
        }

    }
}
