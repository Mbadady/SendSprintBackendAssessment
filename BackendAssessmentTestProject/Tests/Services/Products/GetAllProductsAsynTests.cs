namespace BackendAssessment.Tests.Services.Products
{
    public class GetAllProductsAsynTests : TestBase
    {
        [Test]
        public async Task GetAllProductsAsync_ValidInput_ReturnsSuccessResponseWithProducts()
        {
            // Arrange
            var searchTerm = "test";
            var skip = 0;
            var take = 10;
            var products = new List<Product> { new() };
            var productDtos = new List<ProductDto> { new() };
            Expression<Func<Product, bool>> filter = p => string.IsNullOrEmpty(searchTerm) || p.Name.ToLower().Contains(searchTerm.ToLower());
            _productRepositoryMock.Setup(r => r.GetAllAsync(skip, take, filter, It.IsAny<CancellationToken>())).ReturnsAsync(products);
            _mapperMock.Setup(m => m.Map<IEnumerable<ProductDto>>(products)).Returns(productDtos);

            // Act
            var response = await _productService.GetAllProductsAsync(searchTerm, skip, take);

            // Assert
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Result, Is.EqualTo(productDtos));
            Assert.That(response.Message, Is.EqualTo("Product(s) found successfully"));
        }

        [Test]
        public async Task GetAllProductsAsync_NoProductsFound_ReturnsSuccessResponseWithNoProductMessage()
        {
            // Arrange
            var searchTerm = "test";
            var skip = 0;
            var take = 10;
            var products = new List<Product>();
            Expression<Func<Product, bool>> filter = p => string.IsNullOrEmpty(searchTerm) || p.Name.ToLower().Contains(searchTerm.ToLower());
            _productRepositoryMock.Setup(r => r.GetAllAsync(skip, take, filter, It.IsAny<CancellationToken>())).ReturnsAsync(products);

            // Act
            var response = await _productService.GetAllProductsAsync(searchTerm, skip, take);

            // Assert
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Result, Is.EqualTo("No product found"));
            Assert.That(response.Message, Is.EqualTo("Product(s) found successfully"));
        }

        [Test]
        public async Task GetAllProductsAsync_ExceptionThrown_ReturnsFailureResponse()
        {
            // Arrange
            var searchTerm = "test";
            var skip = 0;
            var take = 10;

            var exceptionMessage = "An error occurred";
            Expression<Func<Product, bool>> filter = p => string.IsNullOrEmpty(searchTerm) || p.Name.ToLower().Contains(searchTerm.ToLower());
            _productRepositoryMock.Setup(r => r.GetAllAsync(skip, take, filter, It.IsAny<CancellationToken>())).Throws(new Exception(exceptionMessage));

            // Act
            var response = await _productService.GetAllProductsAsync(searchTerm, skip, take);

            // Assert
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo($"An error occurred while fetching the products: {exceptionMessage}"));
        }
    }
}
