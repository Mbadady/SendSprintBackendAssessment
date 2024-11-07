using BackendAssessment.Models.DTOs.Order;
using BackendAssessment.Models;
using Moq;
using NUnit.Framework;
using BackendAssessment.Util.Enums;

namespace BackendAssessment.Tests.Services.Orders
{
    [TestFixture]
    public class AddOrderAsyncTests : TestBase
    {
        [Test]
        public async Task AddOrderAsync_Should_Return_SuccessResponse()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                Id = 1,
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

            _mapperMock.Setup(m => m.Map<Order>(orderDto)).Returns(order);
            _orderRepositoryMock.Setup(r => r.AddAsync(order, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _mapperMock.Setup(m => m.Map<OrderDto>(order)).Returns(orderDto);

            // Act
            var response = await _orderService.AddOrderAsync(orderDto);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(response.IsSuccess, Is.True);
                Assert.That(response.Result, Is.EqualTo(orderDto));
                Assert.That(response.Message, Is.EqualTo("Order created successfully"));
            });
        }

        [Test]
        public async Task AddOrderAsync_ExceptionThrown_ReturnsFailureResponse()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                Id = 1,
                TransactionId = 10,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PaymentStatus = PaymentStatus.Approved,
                TotalAmount = 10,
                UserEmail = "example@test.com"
            };

            var exceptionMessage = "An error occurred";
            _mapperMock.Setup(m => m.Map<Order>(orderDto)).Throws(new Exception(exceptionMessage));

            // Act
            var response = await _orderService.AddOrderAsync(orderDto);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(response.IsSuccess, Is.False);
            });
        }

    }
}
