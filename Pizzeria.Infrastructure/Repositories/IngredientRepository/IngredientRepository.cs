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
                
            return JsonSerializer.Deserialize<List<ProductIngredient>>(content) 
                   ?? throw new InvalidOperationException("Failed to deserialize ingredients");
        });
    }
}