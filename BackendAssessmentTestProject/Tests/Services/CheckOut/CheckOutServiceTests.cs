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
        private class CheckOutServiceTestable : CheckOutService
        {
            public PaymentResponse MockedResponse { get; set; }

            public CheckOutServiceTestable(IOrderRepository orderRepository, ITransactionRepository transactionRepository, IProductRepository productRepository, IConfiguration configuration, IMapper mapper)
                : base(orderRepository, transactionRepository, productRepository, configuration, mapper) { }

            protected override PaymentResponse InitiatePayment(Transaction transaction, string email)
            {
                return MockedResponse; // Return the mocked response
            }
        }

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

            // Setup the mock to return the expected value
            _configurationMock.Setup(c => c["Payment:PaystackSK"]).Returns("test_key");
        }

        [Test]
        public async Task ProcessCheckoutAsync_ShouldReturnSuccess_WhenCheckoutIsValid()
        {
            // Arrange
            var checkOutRequest = new CheckOutRequest
            {
                Products =
                [
                    new ProductQuantity { ProductId = 1, Quantity = 2 }
                ]
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
            // Create a successful payment response
            var successfulPaymentResponse = new PaymentResponse
            {
                AuthorizationUrl = "http://payment-url.com",
                PaymentReference = "test_payment_reference"
            };

            // Create testable service and set up mocked response
            var testService = new CheckOutServiceTestable(
                _orderRepositoryMock.Object,
                _transactionRepositoryMock.Object,
                _productRepositoryMock.Object,
                _configurationMock.Object,
                _mapper
            )
            {
                MockedResponse = successfulPaymentResponse // Set the successful response
            };

            // Act
            var result = await testService.ProcessCheckoutAsync(checkOutRequest, "user@example.com");

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Message, Is.EqualTo("Checkout successful. Proceed to confirm payment"));
            });
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

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Message, Is.EqualTo("Product not found for this id 1"));
            });
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

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Message, Is.EqualTo("Product quantity is not enough for this id 1"));
            });
        }

        [Test]
        public async Task ProcessCheckoutAsync_ShouldReturnFailure_WhenPaymentInitializationFails()
        {
            // Arrange
            var checkOutRequest = new CheckOutRequest
            {
                Products =
            [
                new ProductQuantity { ProductId = 1, Quantity = 2 }
            ]
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

            // Create testable service and set up mocked response
            var testService = new CheckOutServiceTestable(
                _orderRepositoryMock.Object,
                _transactionRepositoryMock.Object,
                _productRepositoryMock.Object,
                _configurationMock.Object,
                _mapper
            )
            {
                MockedResponse = new PaymentResponse
                {
                    AuthorizationUrl = string.Empty, // Simulate failure
                    PaymentReference = string.Empty
                }
            };

            // Act
            var result = await testService.ProcessCheckoutAsync(checkOutRequest, "user@example.com");

            // Assert
            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Message, Is.EqualTo("Unable to complete checkout"));
            });
        }
    }
}
