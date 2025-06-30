using Mapster;
using Pizzeria.Application.DTOs.OrderDTOs;
using Pizzeria.Domain.Entities.OrderEntity.Root;

namespace Pizzeria.Application.Mappings;

public static class MappingConfig
{
    public static void ConfigureMappings()
    {
        TypeAdapterConfig<Order, OrderDto>
            .NewConfig()
            .Map(dest => dest.Items, src => src.Items.Select(item =>
                new OrderItemDto(
                    item.ProductId,
                    item.Quantity,
                    "", // ProductName OrderService’de doldurulacak
                    0   // Price da OrderService’de doldurulacak
                )).ToList());
    }
}