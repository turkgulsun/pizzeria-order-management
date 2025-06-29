using Microsoft.Extensions.Logging;
using Pizzeria.Domain.Abstractions.FileAbstractions;
using Polly;

namespace Pizzeria.Infrastructure.Services.FileServices;

public class FileService : IFileService
{
    private readonly ILogger<FileService> _logger;
    private readonly IAsyncPolicy _retryPolicy;

    public FileService(ILogger<FileService> logger)
    {
        _logger = logger;
            
        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt => 
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (exception, timeSpan, retryCount, context) => 
                {
                    _logger.LogWarning(exception, "Retry {RetryCount} for file operation", retryCount);
                });
    }

    public async Task<string> ReadFileContent(string filePath)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Reading file: {FilePath}", filePath);
                
            if (!File.Exists(filePath))
            {
                _logger.LogError("File not found: {FilePath}", filePath);
                throw new FileNotFoundException($"File not found: {filePath}");
            }
                
            return await File.ReadAllTextAsync(filePath);
        });
    }
}