using FluentAssertions;
using Pizzeria.Domain.Entities.IngredientEntity;
using Pizzeria.Domain.Entities.OrderEntity.Root.Entities;

namespace Pizzeria.UnitTests;

public class IngredientTests
{
    [Fact]
    public void Calculate_Total_Ingredients_For_Orders()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var orders = new List<OrderItem>
        {
            new OrderItem(productId, 2) // 2 pizza
        };

        var ingredients = new List<ProductIngredient>
        {
            new ProductIngredient(productId, new List<Ingredient>
            {
                new Ingredient("Flour", 200, "g"),
                new Ingredient("Cheese", 100, "g"),
            })
        };
        
        var totalIngredients = new Dictionary<string, decimal>();
        foreach (var item in orders)
        {
            var ingredientList = ingredients.First(pi => pi.ProductId == item.ProductId).Ingredients;
            foreach (var ing in ingredientList)
            {
                if (!totalIngredients.ContainsKey(ing.Name))
                    totalIngredients[ing.Name] = 0;
                totalIngredients[ing.Name] += ing.Amount * item.Quantity;
            }
        }

        // Assert
        totalIngredients["Flour"].Should().Be(400);
        totalIngredients["Cheese"].Should().Be(200);
    }
}