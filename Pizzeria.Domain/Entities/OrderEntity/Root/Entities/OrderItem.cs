using Pizzeria.Domain.SeedWork;

namespace Pizzeria.Domain.Entities.OrderEntity.Root.Entities;

public class OrderItem : Entity<Guid>
{
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }

    private OrderItem()
    {
    } // ORM için

    public OrderItem(Guid productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }
    
}