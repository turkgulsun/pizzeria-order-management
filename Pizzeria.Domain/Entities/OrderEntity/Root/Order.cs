using Pizzeria.Domain.Entities.OrderEntity.Root.Entities;
using Pizzeria.Domain.Entities.OrderEntity.Root.ValueObjects;
using Pizzeria.Domain.SeedWork;

namespace Pizzeria.Domain.Entities.OrderEntity.Root;

public class Order : Entity<Guid>, IAggregateRoot
{
    private readonly List<OrderItem> _items = new();

    public DateTime CreatedAt { get; private set; }
    public DateTime DeliveryAt { get; private set; }
    public Address DeliveryAddress { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public decimal TotalPrice { get; private set; }

    public static Order Create(
        Guid id,
        DateTime createdAt,
        DateTime deliveryAt,
        Address deliveryAddress,
        List<OrderItem> items)
    {
        var order = new Order(id, createdAt, deliveryAt, deliveryAddress);
        foreach (var item in items)
        {
            order.AddItem(item);
        }

        return order;
    }

    private Order(Guid id, DateTime createdAt, DateTime deliveryAt, Address deliveryAddress)
    {
        Id = id;
        CreatedAt = createdAt;
        DeliveryAt = deliveryAt;
        DeliveryAddress = deliveryAddress;
    }

    protected Order()
    {
    }

    public void AddItem(OrderItem item)
    {
        _items.Add(item);
        TotalPrice += item.UnitPrice * item.Quantity;
    }

    public void RemoveItem(Guid productId)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            _items.Remove(item);
            TotalPrice -= item.UnitPrice * item.Quantity;
        }
    }
}