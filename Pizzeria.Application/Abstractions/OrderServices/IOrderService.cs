using Pizzeria.Application.DTOs.IngredientDTOs;
using Pizzeria.Application.DTOs.OrderDTOs;

namespace Pizzeria.Application.Abstractions.OrderServices;

public interface IOrderService
{
    Task<IEnumerable<OrderDto>> ProcessOrders();
    Task<IEnumerable<IngredientSummaryDto>> CalculateRequiredIngredients();
}