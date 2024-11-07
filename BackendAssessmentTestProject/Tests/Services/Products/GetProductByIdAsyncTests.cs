using BackendAssessment.Models.DTOs.Product;
using BackendAssessment.Models;
using Moq;
using NUnit.Framework;

namespace BackendAssessment.Tests.Services.Products
{
    [TestFixture]
    public class GetProductByIdAsyncTests : TestBase
    {
        [Test]
        public async Task GetProductByIdAsync_ValidInput_ReturnsSuccessResponseWithProduct()
        {
            // Arrange
            var id = 1;
            var product = new Product();
            var productDto = new ProductDto();
            _productRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(product);
            _mapperMock.Setup(m => m.Map<ProductDto>(product)).Returns(productDto);

            // Act
            var response = await _productService.GetProductByIdAsync(id);

            // Assert
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Result, Is.EqualTo(productDto));
            Assert.That(response.Message, Is.EqualTo("Product found successfully"));
        }

        [Test]
        public async Task GetProductByIdAsync_ProductNotFound_ReturnsFailureResponse()
        {
            // Arrange
            var id = 1;
            _productRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Product)null);

            // Act
            var response = await _productService.GetProductByIdAsync(id);

            // Assert
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("Product not found."));
        }

        [Test]
        public async Task GetProductByIdAsync_ExceptionThrown_ReturnsFailureResponse()
        {
            // Arrange
            var id = 1;
            var exceptionMessage = "An error occurred";
            _productRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).Throws(new Exception(exceptionMessage));

            // Act
            var response = await _productService.GetProductByIdAsync(id);

            // Assert
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo($"An error occurred while fetching the product: {exceptionMessage}"));
        }
    }
}
