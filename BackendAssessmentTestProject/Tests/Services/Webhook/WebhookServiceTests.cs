
namespace BackendAssessment.Test.Tests.Services.Webhook
{
    public class WebhookServiceTests : TestBase
    {
        [Test]
        public async Task HandleWebhookAsync_ShouldApproveOrder_WhenChargeSuccessEvent()
        {
            // Arrange
            var transactionReference = "test_transaction_reference";
            var webhookRequest = new WebhookRequest
            {
                Data = new Models.DTOs.Payment.Data { Reference = transactionReference },
                Event = "charge.success"
            };

            var transactionDto = new TransactionDto
            {
                Id = 1,
                PaymentReference = transactionReference,
                Status = PaymentStatus.Pending
            };

            var order = new Order
            {
                Id = 1,
                PaymentStatus = PaymentStatus.Pending,
                PaymentStatusDesc = PaymentStatus.Pending.ToString(),
                OrderItemsJson = JsonSerializer.Serialize(new List<OrderItem>
        {
            new() { ProductId = 1, Quantity = 2 }
        })
            };

            // Mock the transaction repository to return the transaction
            _transactionRepositoryMock.Setup(repo => repo.GetByReferenceAsync(transactionReference))
                .ReturnsAsync(transactionDto);

            // Mock the order repository to return the order
            _orderRepositoryMock.Setup(repo => repo.FindByTransactionIdAsync(It.Is<int>(id => id == transactionDto.Id), It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            // Mock the product repository to verify quantity update
            _productRepositoryMock.Setup(repo => repo.UpdateProductQuantity(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var webhookService = new WebhookService(
                _transactionRepositoryMock.Object,
                _orderRepositoryMock.Object,
                _productRepositoryMock.Object,
                _mapperMock.Object
            );

            // Act
            await webhookService.HandleWebhookAsync(webhookRequest);

            // Assert
            Assert.That(order.PaymentStatus, Is.EqualTo(PaymentStatus.Approved));
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
                Data = new()
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

            var transactionDto = new TransactionDto { Id = 1, Status = PaymentStatus.Pending };
            _transactionRepositoryMock.Setup(repo => repo.GetByReferenceAsync("transaction_ref_123"))
                .ReturnsAsync(transactionDto);
            _orderRepositoryMock.Setup(repo => repo.FindByTransactionIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Order?)null);

            // Act & Assert
            Assert.ThrowsAsync<ResourceNotFoundException>(() => _webhookService.HandleWebhookAsync(webhookRequest));
        }
    }
}
