using BackendAssessment.Models;
using BackendAssessment.Models.DTOs.Product;
using Moq;

namespace BackendAssessment.Tests.Services.Products
{
    public class AddProductAsyncTests : TestBase
    {
        [Test]
        public async Task AddProductAsync_ValidInput_ReturnsSuccessResponse()
        {
            // Arrange
            var createProductDto = new CreateProductDto();
            var product = new Product();
            var productDto = new ProductDto();
            _mapperMock.Setup(m => m.Map<Product>(createProductDto)).Returns(product);
            _productRepositoryMock.Setup(r => r.AddAsync(product, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<ProductDto>(product)).Returns(productDto);
            SetupMockData();


            // Act
            var response = await _productService.AddProductAsync(createProductDto);

            // Assert
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Result, Is.EqualTo(productDto));
            Assert.That(response.Message, Is.EqualTo("Product created successfully"));
        }

        [Test]
        public async Task AddProductAsync_ExceptionThrown_ReturnsFailureResponse()
        {
            // Arrange
            var createProductDto = new CreateProductDto();
            var exceptionMessage = "An error occurred";
            _mapperMock.Setup(m => m.Map<Product>(createProductDto)).Throws(new Exception(exceptionMessage));

            // Act
            var response = await _productService.AddProductAsync(createProductDto);

            // Assert
            Assert.That(response.IsSuccess, Is.False);
        }
    }
}
