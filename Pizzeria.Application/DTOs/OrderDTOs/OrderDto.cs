namespace Pizzeria.Application.DTOs.OrderDTOs;

public record OrderDto(
    Guid Id,
    DateTime CreatedAt,
    DateTime DeliveryAt,
    string DeliveryAddress,
    List<OrderItemDto> Items,
    decimal TotalPrice);

public record OrderItemDto(
    Guid ProductId,
    int Quantity,
    string ProductName,
    decimal Price);