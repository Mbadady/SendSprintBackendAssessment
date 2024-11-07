using BackendAssessment.Exceptions;
using BackendAssessment.Interfaces;
using BackendAssessment.Interfaces.Repositories;
using BackendAssessment.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendAssessment.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(IAppDbContext context) : base(context)
        {
        }

        public async Task UpdateProductQuantity(int productId, int orderQuantity, CancellationToken cancellationToken = default)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId ==  productId, cancellationToken);
            if (product == null)
            {
                throw new ResourceNotFoundException("Product not found.");
            }

            // Subtract order quantity from the product's stock
            product.Quantity -= orderQuantity;
            product.UpdatedAt = DateTime.UtcNow;
            if (product.Quantity < 0)
            {
                throw new InvalidOperationException("Product quantity cannot be negative.");
            }

            _context.Products.Update(product);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
