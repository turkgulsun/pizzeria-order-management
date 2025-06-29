using Pizzeria.Domain.SeedWork;

namespace Pizzeria.Domain.Entities.OrderEntity.Root.Entities;

public class OrderItem : Entity<Guid>
{
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }

    private OrderItem()
    {
    } // ORM için

    public OrderItem(Guid productId, int quantity, decimal unitPrice)
    {
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("Quantity must be positive");

        Quantity = newQuantity;
    }
}