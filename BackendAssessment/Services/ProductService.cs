using AutoMapper;
using BackendAssessment.Interfaces.Repositories;
using BackendAssessment.Interfaces.Services;
using BackendAssessment.Models;
using BackendAssessment.Models.DTOs;
using BackendAssessment.Models.DTOs.Product;
using System.Linq.Expressions;

namespace BackendAssessment.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<ResponseDto> AddProductAsync(CreateProductDto createProductDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var product = _mapper.Map<Product>(createProductDto);
                await _productRepository.AddAsync(product, cancellationToken);

                var productDto = _mapper.Map<ProductDto>(product);

                return ResponseDto.Success(productDto, "Product created successfully");
            }
            catch (Exception ex)
            {
                return ResponseDto.Failure(createProductDto, $"An error occurred while creating the product: {ex.Message}", ex);
            }

        }

        public async Task<ResponseDto> GetAllProductsAsync(string? searchTerm, int? skip, int? take, CancellationToken cancellationToken = default)
        {
            try
            {
                Expression<Func<Product, bool>> filter = p =>
                    string.IsNullOrEmpty(searchTerm) || p.Name.ToLower().Contains(searchTerm.ToLower());

                var products = await _productRepository.GetAllAsync(skip, take, filter, cancellationToken);

                if (!products.Any())
                {
                    return ResponseDto.Success("No product found");
                }

                var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
                return ResponseDto.Success(productDtos, "Product(s) found successfully");
            }
            catch (Exception ex)
            {
                return ResponseDto.Failure($"An error occurred while fetching the products: {ex.Message}", ex);
            }
        }

        public async Task<ResponseDto> GetProductByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id, cancellationToken);
                if (product == null)
                    return ResponseDto.Failure("Product not found.");

                var productDto = _mapper.Map<ProductDto>(product);
                return ResponseDto.Success(productDto, "Product found successfully");
            }
            catch (Exception ex)
            {
                return ResponseDto.Failure($"An error occurred while fetching the product: {ex.Message}", ex);
            }

        }

        public async Task<ResponseDto> UpdateProductAsync(int id, UpdateProductDto updateProductDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var existingProduct = await _productRepository.GetByIdAsync(id, cancellationToken);
                if (existingProduct == null)
                    return ResponseDto.Failure(updateProductDto, "Product not found.");

                _mapper.Map(updateProductDto, existingProduct);

                await _productRepository.UpdateAsync(existingProduct, cancellationToken);
                var productDto = _mapper.Map<ProductDto>(existingProduct);

                return ResponseDto.Success(productDto, "Product updated successfully");
            }
            catch (Exception ex)
            {
                return ResponseDto.Failure(updateProductDto, $"An error occurred while updating the product: {ex.Message}", ex);
            }
        }
        public async Task<ResponseDto> DeleteProductAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id, cancellationToken);
                if (product == null)
                    return ResponseDto.Failure("Product not found.");

                await _productRepository.RemoveAsync(product, cancellationToken);

                return ResponseDto.Success("Product deleted successfully");
            }
            catch (Exception ex)
            {
                return ResponseDto.Failure($"An error occurred while deleting the product: {ex.Message}", ex);
            }

        }
    }
}
