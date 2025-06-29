using Microsoft.Extensions.DependencyInjection;
using Pizzeria.Domain.Abstractions.FileAbstractions;
using Pizzeria.Domain.Abstractions.IngredientAbstractions;
using Pizzeria.Domain.Abstractions.OrderAbstractions;
using Pizzeria.Domain.Abstractions.ProductAbstractions;
using Pizzeria.Infrastructure.Repositories.IngredientRepository;
using Pizzeria.Infrastructure.Repositories.OrderRepository;
using Pizzeria.Infrastructure.Repositories.ProductRepository;
using Pizzeria.Infrastructure.Services.FileServices;

namespace Pizzeria.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IIngredientRepository, IngredientRepository>();
            
        return services;
    }
}