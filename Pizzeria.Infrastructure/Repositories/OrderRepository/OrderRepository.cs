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

            var orderItems = JsonSerializer.Deserialize<List<OrderItemRecord>>(content)
                             ?? throw new InvalidOperationException("Failed to deserialize orders");

            return orderItems
                .GroupBy(x => x.OrderId)
                .Select(g => Order.Create(
                    id: g.Key,
                    createdAt: g.First().CreatedAt,
                    deliveryAt: g.First().DeliveryAt,
                    deliveryAddress: new Address(
                        g.First().Street,
                        g.First().City,
                        g.First().PostalCode),
                    items: g.Select(x => new OrderItem(
                            productId: x.ProductId,
                            quantity: x.Quantity,
                            unitPrice: 0))
                        .ToList()));
        });
    }
    
    private record OrderItemRecord(
        Guid OrderId,
        Guid ProductId,
        int Quantity,
        DateTime CreatedAt,
        DateTime DeliveryAt,
        string Street,
        string City,
        string PostalCode);
}