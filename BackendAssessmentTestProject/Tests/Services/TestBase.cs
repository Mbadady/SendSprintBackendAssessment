using BackendAssessment.Interfaces;
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
        }
    }
}
