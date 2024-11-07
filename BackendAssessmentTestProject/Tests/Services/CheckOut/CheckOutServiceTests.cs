using AutoMapper;
using BackendAssessment.Interfaces.IRepositories;
using BackendAssessment.Interfaces.Repositories;
using BackendAssessment.Models.DTOs.Payment;
using BackendAssessment.Models;
using BackendAssessment.Services;
using Moq;
using Microsoft.Extensions.Configuration;
using BackendAssessment.Models.DTOs.Order;
using BackendAssessment.Models.DTOs.Transaction;
using BackendAssessment.Interfaces.Services;

namespace BackendAssessment.Tests.Services.CheckOut
{
    [TestFixture]
    public class CheckOutServiceTests
    {
        private Mock<IOrderRepository> _orderRepositoryMock;
        private Mock<ITransactionRepository> _transactionRepositoryMock;
        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<IConfiguration> _configurationMock;
        private IMapper _mapper;
        private ICheckOutService _checkOutService;

        [SetUp]
        public void Setup()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _configurationMock = new Mock<IConfiguration>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, OrderDto>();
                cfg.CreateMap<Transaction, TransactionDto>();
            });
            _mapper = config.CreateMapper();

            _checkOutService = new CheckOutService(
                _orderRepositoryMock.Object,
                _transactionRepositoryMock.Object,
                _productRepositoryMock.Object,
                _configurationMock.Object,
                _mapper
            );

            _configurationMock.Setup(c => c.GetValue<string>("Payment:PaystackSK")).Returns("test_key");
        }

        [Test]
        public async Task ProcessCheckoutAsync_ShouldReturnSuccess_WhenCheckoutIsValid()
        {
            // Arrange
            var checkOutRequest = new CheckOutRequest
            {
                Products = new List<ProductQuantity>
                {
                    new ProductQuantity { ProductId = 1, Quantity = 2 }
                }
            };

            var product = new Product
            {
                ProductId = 1,
                Name = "Test Product",
                Price = 1000,
                Quantity = 5
            };

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);
            _orderRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _transactionRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _orderRepositoryMock.Setup(repo => repo.UpdateOrderAsync(It.IsAny<OrderDto>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _checkOutService.ProcessCheckoutAsync(checkOutRequest, "user@example.com");

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Message, Is.EqualTo("Checkout successful. Proceed to confirm payment"));
            _orderRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);
            _transactionRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ProcessCheckoutAsync_ShouldReturnFailure_WhenProductNotFound()
        {
            // Arrange
            var checkOutRequest = new CheckOutRequest
            {
                Products =
                [
                    new ProductQuantity { ProductId = 1, Quantity = 2 }
                ]
            };

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null); // Simulate product not found

            // Act
            var result = await _checkOutService.ProcessCheckoutAsync(checkOutRequest, "user@example.com");

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("Product not found for this id 1"));
        }

        [Test]
        public async Task ProcessCheckoutAsync_ShouldReturnFailure_WhenInsufficientProductQuantity()
        {
            // Arrange
            var checkOutRequest = new CheckOutRequest
            {
                Products =
                [
                    new ProductQuantity { ProductId = 1, Quantity = 10 } // Requesting more than available
                ]
            };

            var product = new Product
            {
                ProductId = 1,
                Name = "Test Product",
                Price = 1000,
                Quantity = 5 // Available quantity is less than requested
            };

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            // Act
            var result = await _checkOutService.ProcessCheckoutAsync(checkOutRequest, "user@example.com");

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("Product quantity is not enough for this id 1"));
        }
    }
}
