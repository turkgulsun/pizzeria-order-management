using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Pizzeria.Application.Abstractions.OrderServices;
using Pizzeria.Application.Services.OrderServices;
using Pizzeria.Domain.Entities.OrderEntity.Root;
using Pizzeria.Domain.Entities.OrderEntity.Validators;

namespace Pizzeria.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IValidator<Order>, OrderValidator>();
        services.AddScoped<IOrderService, OrderService>();
            
        return services;
    }
}