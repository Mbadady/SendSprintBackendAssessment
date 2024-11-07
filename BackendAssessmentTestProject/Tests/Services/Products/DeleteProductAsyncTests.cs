namespace BackendAssessment.Tests.Services.Products
{
    public class DeleteProductAsyncTests : TestBase
    {
        [Test]
        public async Task DeleteProductAsync_ValidInput_ReturnsSuccessResponse()
        {
            // Arrange
            var id = 1;
            var product = new Product();
            _productRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(product);
            _productRepositoryMock.Setup(r => r.RemoveAsync(product, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            var response = await _productService.DeleteProductAsync(id);

            // Assert
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Message, Is.EqualTo("Product deleted successfully"));
        }

        [Test]
        public async Task DeleteProductAsync_ProductNotFound_ReturnsFailureResponse()
        {
            // Arrange
            var id = 1;
            _productRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Product)null);

            // Act
            var response = await _productService.DeleteProductAsync(id);

            // Assert
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("Product not found."));
        }

        [Test]
        public async Task DeleteProductAsync_ExceptionThrown_ReturnsFailureResponse()
        {
            // Arrange
            var id = 1;
            var exceptionMessage = "An error occurred";
            _productRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).Throws(new Exception(exceptionMessage));

            // Act
            var response = await _productService.DeleteProductAsync(id);

            // Assert
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo($"An error occurred while deleting the product: {exceptionMessage}"));
        }
    }
}
