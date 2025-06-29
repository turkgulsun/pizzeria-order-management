using FluentValidation;
using Mapster;
using Microsoft.Extensions.Logging;
using Pizzeria.Application.Abstractions.OrderServices;
using Pizzeria.Application.DTOs.IngredientDTOs;
using Pizzeria.Application.DTOs.OrderDTOs;
using Pizzeria.Domain.Abstractions.IngredientAbstractions;
using Pizzeria.Domain.Abstractions.OrderAbstractions;
using Pizzeria.Domain.Abstractions.ProductAbstractions;
using Pizzeria.Domain.Entities.OrderEntity.Root;
using Pizzeria.Domain.Entities.OrderEntity.Validators;
using Pizzeria.Domain.Entities.ProductEntity;

namespace Pizzeria.Application.Services.OrderServices;

public class OrderService(IOrderRepository orderRepository,
    IProductRepository productRepository,
    IIngredientRepository ingredientRepository,
    IValidator<Order> orderValidator,
    ILogger<OrderService> logger) : IOrderService
{
    public async Task<IEnumerable<OrderDto>> ProcessOrders()
    {
        logger.LogInformation("Starting order processing");

        var orders = await orderRepository.GetAllOrders();
        var products = await productRepository.GetAllProducts();
        var productIngredients = await ingredientRepository.GetAllProductIngredients();

        var validOrders = new List<OrderDto>();

        foreach (var order in orders)
        {
            var validationResult = await orderValidator.ValidateAsync(order);
            if (!validationResult.IsValid)
            {
                logger.LogWarning("Invalid Order {OrderId}. Errors: {Errors}",
                    order.Id, string.Join("; ", validationResult.Errors));
                return null;
            }

            // Calculate total price
            decimal totalPrice = 0;
            var orderItems = new List<OrderItemDto>();

            foreach (var item in order.Items)
            {
                var product = products.FirstOrDefault(p => p.ProductId == item.ProductId);
                if (product == null)
                {
                    logger.LogWarning("Product {ProductId} not found for order {OrderId}",
                        item.ProductId, order.Id);
                    continue;
                }

                totalPrice += product.Price * item.Quantity;
                orderItems.Add(new OrderItemDto(
                    item.ProductId,
                    item.Quantity,
                    product.ProductName,
                    product.Price));
            }

            var orderDto = order.Adapt<OrderDto>();
            orderDto = orderDto with
            {
                Items = orderItems,
                TotalPrice = totalPrice
            };

            validOrders.Add(orderDto);
        }

        logger.LogInformation("Processed {ValidCount} valid orders out of {TotalCount}",
            validOrders.Count, orders.Count());

        return validOrders;
    }

    private async Task<OrderDto> ProcessSingleOrder(Order order, IEnumerable<Product> products)
    {
        decimal totalPrice = 0;
        var orderItems = new List<OrderItemDto>();

        foreach (var item in order.Items)
        {
            var product = products.FirstOrDefault(p => p.ProductId == item.ProductId)
                          ?? throw new KeyNotFoundException($"Product {item.ProductId} not found");

            totalPrice += product.Price * item.Quantity;
            orderItems.Add(new OrderItemDto(
                item.ProductId,
                item.Quantity,
                product.ProductName,
                product.Price));
        }

        return order.Adapt<OrderDto>() with
        {
            Items = orderItems,
            TotalPrice = totalPrice
        };
    }

    public async Task<IEnumerable<IngredientSummaryDto>> CalculateRequiredIngredients()
    {
        logger.LogInformation("Calculating required ingredients");

        var orders = (await orderRepository.GetAllOrders())
            .Where(o => new OrderValidator().Validate(o).IsValid);

        var productIngredients = await ingredientRepository.GetAllProductIngredients();

        var ingredientSummary = new Dictionary<string, (decimal Amount, string Unit)>();

        foreach (var order in orders)
        {
            foreach (var item in order.Items)
            {
                var ingredients = productIngredients
                    .FirstOrDefault(pi => pi.ProductId == item.ProductId)?
                    .Ingredients;

                if (ingredients == null) continue;

                foreach (var ingredient in ingredients)
                {
                    if (ingredientSummary.ContainsKey(ingredient.Name))
                    {
                        var current = ingredientSummary[ingredient.Name];
                        ingredientSummary[ingredient.Name] =
                            (current.Amount + (ingredient.Amount * item.Quantity), ingredient.Unit);
                    }
                    else
                    {
                        ingredientSummary[ingredient.Name] =
                            (ingredient.Amount * item.Quantity, ingredient.Unit);
                    }
                }
            }
        }

        return ingredientSummary.Select(kvp =>
            new IngredientSummaryDto(kvp.Key, kvp.Value.Amount, kvp.Value.Unit));
    }
}