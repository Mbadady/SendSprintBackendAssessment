using BackendAssessment.Interfaces.IRepositories;
using BackendAssessment.Models;

namespace BackendAssessment.Interfaces.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task UpdateProductQuantity(int productId, int orderQuantity, CancellationToken cancellationToken = default);
    }
}
