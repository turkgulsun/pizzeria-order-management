using Pizzeria.Application.Abstractions.OrderServices;
using Pizzeria.Application.Mappings;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pizzeria.Application;
using Pizzeria.Infrastructure;

// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/pizzeria.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting Pizzeria Order Processor");

    // Configure host
    var host = Host.CreateDefaultBuilder(args)
        .ConfigureServices((context, services) =>
        {
            // Add layers
            services.AddInfrastructure();
            services.AddApplication();

            // Configure mappings
            MappingConfig.ConfigureMappings();

            // Add logging
            services.AddLogging(configure =>
                configure.AddSerilog());
        })
        .Build();

    var orderService = host.Services.GetRequiredService<IOrderService>();

    // Process orders
    var orders = await orderService.ProcessOrders();

    Console.WriteLine("=== Valid Orders ===");
    foreach (var order in orders)
    {
        Console.WriteLine($"Order ID: {order.Id}");
        Console.WriteLine($"Delivery Address: {order.DeliveryAddress}");
        Console.WriteLine($"Created At: {order.CreatedAt:yyyy-MM-dd HH:mm:ss}");
        Console.WriteLine($"Delivery At: {order.DeliveryAt:yyyy-MM-dd HH:mm:ss}");
        Console.WriteLine($"Total Price: {order.TotalPrice:C}");

        Console.WriteLine("Items:");
        foreach (var item in order.Items)
        {
            Console.WriteLine($"  - Product: {item.ProductId}, Quantity: {item.Quantity}, Unit Price: {item.Price:C}");
        }

        Console.WriteLine();
    }

    // Calculate and display ingredients
    var ingredients = await orderService.CalculateRequiredIngredients();

    Console.WriteLine("=== Required Ingredients ===");
    foreach (var ingredient in ingredients)
    {
        Console.WriteLine($"{ingredient.IngredientName}: {ingredient.TotalAmount} {ingredient.Unit}");
    }

    Log.Information("Order processing completed successfully");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}