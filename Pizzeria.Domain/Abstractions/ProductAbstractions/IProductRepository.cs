using Pizzeria.Domain.Entities.ProductEntity;

namespace Pizzeria.Domain.Abstractions.ProductAbstractions;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllProducts();
}