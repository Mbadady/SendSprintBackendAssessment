using BackendAssessment.Models;
using BackendAssessment.Models.DTOs;
using BackendAssessment.Models.DTOs.Product;

namespace BackendAssessment.Interfaces.Services
{
    public interface IProductService
    {
        Task<ResponseDto> GetProductByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<ResponseDto> GetAllProductsAsync(string? searchTerm, int? skip, int? take, CancellationToken cancellationToken = default);
        Task<ResponseDto> AddProductAsync(CreateProductDto createProductDto, CancellationToken cancellationToken = default);
        Task<ResponseDto> UpdateProductAsync(int id, UpdateProductDto updateProductDto, CancellationToken cancellationToken = default);
        Task<ResponseDto> DeleteProductAsync(int id, CancellationToken cancellationToken = default);
    }
}
