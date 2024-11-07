using BackendAssessment.Interfaces.Services;
using BackendAssessment.Models.DTOs;
using BackendAssessment.Models.DTOs.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendAssessment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createProductDto, CancellationToken cancellationToken)
        {
            var result = await _productService.AddProductAsync(createProductDto, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 404)]
        public async Task<IActionResult> GetAllProducts([FromQuery] string? searchTerm, [FromQuery] int? skip, [FromQuery] int? take, CancellationToken cancellationToken)
        {
            var result = await _productService.GetAllProductsAsync(searchTerm, skip, take, cancellationToken);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 404)]
        public async Task<IActionResult> GetProductById(int id, CancellationToken cancellationToken)
        {
            var result = await _productService.GetProductByIdAsync(id, cancellationToken);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 404)]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto productDto, CancellationToken cancellationToken)
        {
            var result = await _productService.UpdateProductAsync(id, productDto, cancellationToken);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 404)]
        public async Task<IActionResult> DeleteProduct(int id, CancellationToken cancellationToken)
        {
            var result = await _productService.DeleteProductAsync(id, cancellationToken);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
    }
}
