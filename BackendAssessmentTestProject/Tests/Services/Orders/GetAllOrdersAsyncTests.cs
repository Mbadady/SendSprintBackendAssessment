using BackendAssessment.Models.DTOs.Order;
using BackendAssessment.Models;
using BackendAssessment.Util.Enums;
using Moq;
using NUnit.Framework;
using System.Linq.Expressions;

namespace BackendAssessment.Tests.Services.Orders
{
    public class GetAllOrdersAsyncTests : TestBase
    {
        [Test]
        public async Task GetAllOrdersAsync_Should_Return_SuccessResponse_WithOrders()
        {
            // Arrange
            var searchTerm = "test";
            var skip = 0;
            var take = 10;
            var orders = new List<Order>
    {
        new Order
        {
            Id = 1,
            UserEmail = "example@test.com",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            PaymentStatus = PaymentStatus.APPROVED,
            TotalAmount = 100
        }
    };
            var orderDtos = new List<OrderDto>
    {
        new OrderDto
        {
            Id = 1,
            UserEmail = "example@test.com",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            PaymentStatus = PaymentStatus.APPROVED,
            TotalAmount = 100
        }
    };
            Expression<Func<Order, bool>> filter = o => string.IsNullOrEmpty(searchTerm) || o.UserEmail.ToLower().Contains(searchTerm.ToLower());
            _orderRepositoryMock.Setup(r => r.GetAllAsync(skip, take, filter, It.IsAny<CancellationToken>())).ReturnsAsync(orders);
            _mapperMock.Setup(m => m.Map<IEnumerable<OrderDto>>(orders)).Returns(orderDtos);

            // Act
            var response = await _orderService.GetAllOrdersAsync(searchTerm, skip, take);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Result, Is.EqualTo(orderDtos));
            Assert.That(response.Message, Is.EqualTo("Order(s) found successfully"));
        }

        [Test]
        public async Task GetAllOrdersAsync_Should_Return_SuccessResponse_WithNoOrderMessage_When_NoOrdersFound()
        {
            // Arrange
            var searchTerm = "test";
            var skip = 0;
            var take = 10;
            var orders = new List<Order>();
            Expression<Func<Order, bool>> filter = o => string.IsNullOrEmpty(searchTerm) || o.UserEmail.ToLower().Contains(searchTerm.ToLower());
            _orderRepositoryMock.Setup(r => r.GetAllAsync(skip, take, filter, It.IsAny<CancellationToken>())).ReturnsAsync(orders);

            // Act
            var response = await _orderService.GetAllOrdersAsync(searchTerm, skip, take);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Result, Is.EqualTo("No order found"));
            Assert.That(response.Message, Is.EqualTo("Order(s) found successfully"));
        }

        [Test]
        public async Task GetAllOrdersAsync_Should_Return_FailureResponse_When_ExceptionThrown()
        {
            // Arrange
            var searchTerm = "test";
            var skip = 0;
            var take = 10;
            var exceptionMessage = "An error occurred";
            Expression<Func<Order, bool>> filter = o => string.IsNullOrEmpty(searchTerm) || o.UserEmail.ToLower().Contains(searchTerm.ToLower());
            _orderRepositoryMock.Setup(r => r.GetAllAsync(skip, take, filter, It.IsAny<CancellationToken>())).Throws(new Exception(exceptionMessage));

            // Act
            var response = await _orderService.GetAllOrdersAsync(searchTerm, skip, take);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo($"An error occurred while fetching the orders: {exceptionMessage}"));
        }

    }
}
