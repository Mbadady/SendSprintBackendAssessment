using BackendAssessment.Models.DTOs.Order;
using BackendAssessment.Models.DTOs.Payment;
using BackendAssessment.Models.DTOs.Transaction;
using BackendAssessment.Models;
using BackendAssessment.Tests.Services;
using BackendAssessment.Util.Enums;
using Moq;
using System.Text.Json;
using BackendAssessment.Exceptions;

namespace BackendAssessment.Test.Tests.Services.WebhookService
{
    public class WebhookServiceTests : TestBase
    {
        [Test]
        public async Task HandleWebhookAsync_ShouldApproveOrder_WhenChargeSuccessEvent()
        {
            // Arrange
            var webhookRequest = new WebhookRequest
            {
                Event = "charge.success",
                Data = new()
                {
                    Reference = "transaction_ref",
                    Email = "test@example.com"
                }
            };

            var transactionDto = new TransactionDto { Id = 1, Status = PaymentStatus.PENDING };
            var order = new Order
            {
                Id = 1,
                PaymentStatus = PaymentStatus.PENDING,
                OrderItemsJson = JsonSerializer.Serialize(new List<OrderItem>
                                {
                                    new() { ProductId = 1, Quantity = 2 }
                                })
            };

            _transactionRepositoryMock.Setup(repo => repo.GetByReferenceAsync("transaction_ref_123"))
                .ReturnsAsync(transactionDto);
            _orderRepositoryMock.Setup(repo => repo.FindByTransactionIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            // Act
            await _webhookService.HandleWebhookAsync(webhookRequest);

            // Assert
            Assert.That(order.PaymentStatus, Is.EqualTo(PaymentStatus.APPROVED));
            _transactionRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()), Times.Once);
            _orderRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);
            _productRepositoryMock.Verify(repo => repo.UpdateProductQuantity(1, 2, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void HandleWebhookAsync_ShouldThrowResourceNotFoundException_WhenTransactionNotFound()
        {
            // Arrange
            var webhookRequest = new WebhookRequest
            {
                Event = "charge.success",
                Data = new ()
                {
                    Reference = "invalid_transaction_ref",
                    Email = "test@example.com"
                }
            };

            _transactionRepositoryMock.Setup(repo => repo.GetByReferenceAsync("invalid_transaction_ref"))
                .ReturnsAsync((TransactionDto?)null);

            // Act & Assert
            Assert.ThrowsAsync<ResourceNotFoundException>(() => _webhookService.HandleWebhookAsync(webhookRequest));
        }

        [Test]
        public void HandleWebhookAsync_ShouldThrowResourceNotFoundException_WhenOrderNotFound()
        {
            // Arrange
            var webhookRequest = new WebhookRequest
            {
                Event = "charge.success",
                Data = new()
                {
                    Reference = "transaction_ref_123",
                    Email = "test@example.com"
                }
            };

            var transactionDto = new TransactionDto { Id = 1, Status = PaymentStatus.PENDING };
            _transactionRepositoryMock.Setup(repo => repo.GetByReferenceAsync("transaction_ref_123"))
                .ReturnsAsync(transactionDto);
            _orderRepositoryMock.Setup(repo => repo.FindByTransactionIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Order?)null);

            // Act & Assert
            Assert.ThrowsAsync<ResourceNotFoundException>(() => _webhookService.HandleWebhookAsync(webhookRequest));
        }
    }
}
