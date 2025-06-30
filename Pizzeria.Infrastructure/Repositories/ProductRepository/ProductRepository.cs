using System.Text.Json;
using Microsoft.Extensions.Logging;
using Pizzeria.Domain.Abstractions.FileAbstractions;
using Pizzeria.Domain.Abstractions.ProductAbstractions;
using Pizzeria.Domain.Entities.ProductEntity;
using Polly;

namespace Pizzeria.Infrastructure.Repositories.ProductRepository;

public class ProductRepository : IProductRepository
{
    private readonly IFileService _fileService;
    private readonly ILogger<ProductRepository> _logger;
    private readonly IAsyncPolicy _retryPolicy;

    public ProductRepository(IFileService fileService, ILogger<ProductRepository> logger)
    {
        _fileService = fileService;
        _logger = logger;

        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Fetching all products");
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "products.json");
            var content = await _fileService.ReadFileContent(filePath);
            
            var productDtos = JsonSerializer.Deserialize<List<ProductDto>>(content)
                            ?? throw new InvalidOperationException("Failed to deserialize products");

            var products = productDtos.Select(dto =>
                new Product(
                    id: dto.ProductId,
                    productName:dto.ProductName,
                    price:dto.Price
                )
            ).ToList();

            return products;
        });
    }

    private class ProductDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }
}