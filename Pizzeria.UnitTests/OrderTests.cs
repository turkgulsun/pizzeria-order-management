using FluentAssertions;
using Pizzeria.Domain.Entities.OrderEntity.Root;
using Pizzeria.Domain.Entities.OrderEntity.Root.Entities;
using Pizzeria.Domain.Entities.OrderEntity.Root.ValueObjects;
using Pizzeria.Domain.Entities.OrderEntity.Validators;
using Pizzeria.Domain.Entities.ProductEntity;

namespace Pizzeria.UnitTests;

public class OrderTests
{
    [Fact]
    public void Order_With_Missing_Street_Should_Be_Invalid()
    {
        // Arrange
        var order = new Order(
            id: Guid.NewGuid(),
            createdAt: DateTime.Now,
            deliveryAt: DateTime.Now.AddHours(2),
            deliveryAddress: new Address("", "City", "12345"), // Street eksik!
            items: new List<OrderItem> { new OrderItem(Guid.NewGuid(), 1) }
        );
        var validator = new OrderValidator();

        // Act
        var result = validator.Validate(order);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName.Contains("Street"));
    }
    
    [Fact]
    public void Order_TotalPrice_Should_Be_Sum_Of_Item_Prices()
    {
        // Arrange
        var productId1 = Guid.NewGuid();
        var productId2 = Guid.NewGuid();
        var products = new List<Product>
        {
            new Product(productId1, "Pizza1", 10),
            new Product(productId2, "Pizza2", 15)
        };
        var items = new List<OrderItem>
        {
            new OrderItem(productId1, 2), // 2 x 10
            new OrderItem(productId2, 1)  // 1 x 15
        };
        var order = new Order(Guid.NewGuid(), DateTime.Now, DateTime.Now.AddMinutes(30), new Address("S", "C", "P"), items);

        // Simüle: OrderService ya da Total hesaplayan metot
        decimal total = 0;
        foreach (var item in order.Items)
        {
            var product = products.First(p => p.Id == item.ProductId);
            total += product.Price * item.Quantity;
        }

        // Assert
        total.Should().Be(35);
    }
}