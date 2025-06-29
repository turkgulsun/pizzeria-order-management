namespace Pizzeria.Domain.Entities.ProductEntity;

public class Product
{
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
}