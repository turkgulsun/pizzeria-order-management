using Pizzeria.Domain.SeedWork;

namespace Pizzeria.Domain.Entities.ProductEntity;

public class Product : Entity<Guid>
{
    public string ProductName { get; private set; } = string.Empty;
    public decimal Price { get; private set; }

    public Product(Guid id, string productName, decimal price)
    {
        Id = id;
        ProductName = productName;
        Price = price;
    }

    protected Product()
    {
    }
}