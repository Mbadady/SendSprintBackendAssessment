using BackendAssessment.Models.DTOs.Product;
using BackendAssessment.Models;
using Moq;
using NUnit.Framework;

namespace BackendAssessment.Tests.Services.Products
{
    public class UpdateProductAsyncTests : TestBase
    {
        [Test]
        public async Task UpdateProductAsync_ValidInput_ReturnsSuccessResponseWithUpdatedProduct()
        {
            // Arrange
            var id = 1;
            var updateProductDto = new UpdateProductDto();
            var existingProduct = new Product();
            var productDto = new ProductDto();
            _productRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(existingProduct);
            _mapperMock.Setup(m => m.Map(updateProductDto, existingProduct));
            _productRepositoryMock.Setup(r => r.UpdateAsync(existingProduct, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<ProductDto>(existingProduct)).Returns(productDto);

            // Act
            var response = await _productService.UpdateProductAsync(id, updateProductDto);

            // Assert
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Result, Is.EqualTo(productDto));
            Assert.That(response.Message, Is.EqualTo("Product updated successfully"));
        }

        [Test]
        public async Task UpdateProductAsync_ProductNotFound_ReturnsFailureResponse()
        {
            // Arrange
            var id = 1;
            var updateProductDto = new UpdateProductDto();
            _productRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Product)null);

            // Act
            var response = await _productService.UpdateProductAsync(id, updateProductDto);

            // Assert
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Result, Is.EqualTo(updateProductDto));
            Assert.That(response.Message, Is.EqualTo("Product not found."));
        }

        [Test]
        public async Task UpdateProductAsync_ExceptionThrown_ReturnsFailureResponse()
        {
            // Arrange
            var id = 1;
            var updateProductDto = new UpdateProductDto();
            var exceptionMessage = "An error occurred";
            _productRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).Throws(new Exception(exceptionMessage));

            // Act
            var response = await _productService.UpdateProductAsync(id, updateProductDto);

            // Assert
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Result, Is.EqualTo(updateProductDto));
            Assert.That(response.Message, Is.EqualTo($"An error occurred while updating the product: {exceptionMessage}"));
        }
    }
}
