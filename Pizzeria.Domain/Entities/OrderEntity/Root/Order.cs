using Pizzeria.Domain.Entities.OrderEntity.Root.Entities;
using Pizzeria.Domain.Entities.OrderEntity.Root.ValueObjects;
using Pizzeria.Domain.SeedWork;

namespace Pizzeria.Domain.Entities.OrderEntity.Root;

public class Order : Entity<Guid>, IAggregateRoot
{
    public DateTime CreatedAt { get; private set; }
    public DateTime DeliveryAt { get; private set; }
    public Address DeliveryAddress { get; private set; }
    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public decimal TotalPrice { get; private set; }
    
    public Order(Guid id, DateTime createdAt, DateTime deliveryAt, Address deliveryAddress, List<OrderItem> items)
    {
        Id = id;
        CreatedAt = createdAt;
        DeliveryAt = deliveryAt;
        DeliveryAddress = deliveryAddress;
        _items = items ?? new();
    }

    protected Order()
    {
    }
}