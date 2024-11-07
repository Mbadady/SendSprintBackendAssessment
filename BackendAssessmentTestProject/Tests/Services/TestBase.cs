using AutoMapper;
using BackendAssessment.Interfaces;
using BackendAssessment.Interfaces.IRepositories;
using BackendAssessment.Interfaces.Repositories;
using BackendAssessment.Interfaces.Services;
using BackendAssessment.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.EntityFrameworkCore;


namespace BackendAssessment.Tests.Services
{
    public class TestBase
    {
        protected Mock<IAppDbContext> _contextMock;
        protected Mock<IOrderRepository> _orderRepositoryMock;
        protected Mock<IMapper> _mapperMock;
        protected Mock<IProductRepository> _productRepositoryMock;
        protected Mock<ITransactionRepository> _transactionRepositoryMock;
        protected IOrderService _orderService;
        protected IProductService _productService;
        protected IWebhookService _webhookService;
        //protected Mock<IConfiguration> _configurationMock;
        //protected Mock<PayStackApi> _payStackApiMock;
        //protected CheckOutService _checkOutService;
        [SetUp]
        public void Setup()
        {
            _contextMock = new Mock<IAppDbContext>();
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _mapperMock = new Mock<IMapper>();
            //_configurationMock = new Mock<IConfiguration>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _orderService = new OrderService(_contextMock.Object, _orderRepositoryMock.Object, _mapperMock.Object);
            _productService = new ProductService(_productRepositoryMock.Object, _mapperMock.Object);
            _webhookService = new WebhookService(_transactionRepositoryMock.Object, _orderRepositoryMock.Object, _productRepositoryMock.Object, _mapperMock.Object);
            //_configurationMock.Setup(c => c.GetValue<string>("Payment:PaystackSK")).Returns("mockToken");
            //_checkOutService = new CheckOutService(
            //    _orderRepositoryMock.Object,
            //    _transactionRepositoryMock.Object,
            //    _productRepositoryMock.Object,
            //    _configurationMock.Object,
            //    _mapperMock.Object
            //);
            SetupMockData();
        }

        public void SetupMockData()
        {

            var orders = DbContextMock.GetQueryableMockDbSet(ContextFakeGenerator.GetOrders());
            var products = DbContextMock.GetQueryableMockDbSet(ContextFakeGenerator.GetProducts());
            var transactions = DbContextMock.GetQueryableMockDbSet(ContextFakeGenerator.GetTransactions());

            _contextMock.Setup(x => x.Orders).ReturnsDbSet(orders);
            _contextMock.Setup(x => x.Products).ReturnsDbSet(products);
            _contextMock.Setup(x => x.Transactions).ReturnsDbSet(transactions);
        }
    }
}
