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
            var searchTerm = "user@example.com"; 
            var skip = 0;
            var take = 10;

            var orders = new List<Order>
                                                {
                                                    new() { UserEmail = "user@example.com", Id = 2 },
                                                    new() { UserEmail = "anotheruser@example.com",Id = 1}
                                                };

            // Mock the repository to return the expected orders
            _orderRepositoryMock.Setup(repo => repo.GetAllAsync(skip, take, It.IsAny<Expression<Func<Order, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(orders);

            var orderDtos = new List<OrderDto>
        {
            new() {Id = 1, PaymentStatus = PaymentStatus.Pending, TotalAmount = 1000, TransactionId = 2,UserEmail = "user@example.com"},
            new() { Id = 1, PaymentStatus = PaymentStatus.Pending, TotalAmount = 1000, TransactionId = 2,UserEmail = "user@example.com" }
        };

            // Mock the mapper to return the DTOs
            _mapperMock.Setup(m => m.Map<IEnumerable<OrderDto>>(orders)).Returns(orderDtos);

            // Act
            var response = await _orderService.GetAllOrdersAsync(searchTerm, skip, take);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(response.IsSuccess, Is.True);
                Assert.That(response.Result, Is.EqualTo(orderDtos));
                Assert.That(response.Message, Is.EqualTo("Order(s) found successfully"));
            });
        }

        [Test]
        public async Task GetAllOrdersAsync_Should_Return_SuccessResponse_WithNoOrderMessage_When_NoOrdersFound()
        {
            // Arrange
            var searchTerm = "user@example.com"; // The email to search for
            var skip = 0;
            var take = 10;

            // Mock the repository to return no orders
            _orderRepositoryMock.Setup(repo => repo.GetAllAsync(skip, take, It.IsAny<Expression<Func<Order, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync([]);

            // Act
            var response = await _orderService.GetAllOrdersAsync(searchTerm, skip, take);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(response.IsSuccess, Is.True);
                Assert.That(response.Message, Is.EqualTo("No order found"));
            });
        }

        [Test]
        public async Task GetAllOrdersAsync_Should_Return_FailureResponse_When_ExceptionThrown()
        {
            // Arrange
            var searchTerm = "user@example.com"; // The email to search for
            var skip = 0;
            var take = 10;

            // Mock the repository to throw an exception
            var exceptionMessage = "Database error";
            _orderRepositoryMock.Setup(repo => repo.GetAllAsync(skip, take, It.IsAny<Expression<Func<Order, bool>>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var response = await _orderService.GetAllOrdersAsync(searchTerm, skip, take);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(response.IsSuccess, Is.False);
                Assert.That(response.Message, Is.EqualTo($"An error occurred while fetching the orders: {exceptionMessage}"));
            });
        }

    }
}
