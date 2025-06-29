namespace Pizzeria.Domain.Entities.IngredientEntity;

public class Ingredient
{
    public string Name { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public string Unit { get; private set; } = "g";
}

public class ProductIngredient
{
    public Guid ProductId { get; private set; }
    public List<Ingredient> Ingredients { get; private set; } = new();
}