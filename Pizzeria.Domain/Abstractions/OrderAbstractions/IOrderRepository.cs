using Pizzeria.Domain.Entities.OrderEntity.Root;

namespace Pizzeria.Domain.Abstractions.OrderAbstractions;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllOrders();
}