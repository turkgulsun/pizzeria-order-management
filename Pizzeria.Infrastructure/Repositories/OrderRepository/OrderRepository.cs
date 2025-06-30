using System.Text.Json;
using Microsoft.Extensions.Logging;
using Pizzeria.Domain.Abstractions.FileAbstractions;
using Pizzeria.Domain.Abstractions.OrderAbstractions;
using Pizzeria.Domain.Entities.OrderEntity.Root;
using Pizzeria.Domain.Entities.OrderEntity.Root.Entities;
using Pizzeria.Domain.Entities.OrderEntity.Root.ValueObjects;
using Polly;

namespace Pizzeria.Infrastructure.Repositories.OrderRepository;

public class OrderRepository : IOrderRepository
{
    private readonly IFileService _fileService;
    private readonly ILogger<OrderRepository> _logger;
    private readonly IAsyncPolicy _retryPolicy;

    public OrderRepository(IFileService fileService, ILogger<OrderRepository> logger)
    {
        _fileService = fileService;
        _logger = logger;

        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (exception, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning(exception, "Retry {RetryCount} for {PolicyKey}", retryCount, context.PolicyKey);
                });
    }

    public async Task<IEnumerable<Order>> GetAllOrders()
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Fetching all orders");
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "orders.json");
            var content = await _fileService.ReadFileContent(filePath);
            
            var orderDtos = JsonSerializer.Deserialize<List<OrderDto>>(content)
                            ?? throw new InvalidOperationException("Failed to deserialize orders");

            var orders = orderDtos.Select(dto =>
                new Order(
                    id: dto.OrderId,
                    createdAt: dto.CreatedAt,
                    deliveryAt: dto.DeliveryAt,
                    deliveryAddress: new Address(dto.Street, dto.City, dto.PostalCode),
                    items: dto.Items.Select(item => new OrderItem(item.ProductId, item.Quantity)).ToList()
                )
            ).ToList();

            return orders;
        });
    }
    
    private class OrderDto
    {
        public Guid OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DeliveryAt { get; set; }
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public List<OrderItemDto> Items { get; set; } = new();
    }
    
    private class OrderItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
    
}