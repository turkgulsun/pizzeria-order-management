using System.Text.Json;
using Microsoft.Extensions.Logging;
using Pizzeria.Domain.Abstractions.FileAbstractions;
using Pizzeria.Domain.Abstractions.IngredientAbstractions;
using Pizzeria.Domain.Entities.IngredientEntity;
using Polly;

namespace Pizzeria.Infrastructure.Repositories.IngredientRepository;

public class IngredientRepository : IIngredientRepository
{
    private readonly IFileService _fileService;
    private readonly ILogger<IngredientRepository> _logger;
    private readonly IAsyncPolicy _retryPolicy;

    public IngredientRepository(IFileService fileService, ILogger<IngredientRepository> logger)
    {
        _fileService = fileService;
        _logger = logger;

        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    public async Task<IEnumerable<ProductIngredient>> GetAllProductIngredients()
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Fetching all product ingredients");
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "ingredients.json");
            var content = await _fileService.ReadFileContent(filePath);

            var productIngredientDtos = JsonSerializer.Deserialize<List<ProductIngredientDto>>(content)
                                        ?? throw new InvalidOperationException("Failed to deserialize ingredients");
            
            var productIngredients = productIngredientDtos.Select(dto =>
                new ProductIngredient(
                    productId: dto.ProductId,
                    ingredients: dto.Ingredients
                        .Select(i => new Ingredient(i.Name, i.Amount, i.Unit))
                        .ToList()
                )
            ).ToList();

            return productIngredients;
                
               
        });
    }

    private class IngredientDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Unit { get; set; } = "g";
    }

    private class ProductIngredientDto
    {
        public Guid ProductId { get; set; }
        public List<IngredientDto> Ingredients { get; set; } = new();
    }
}