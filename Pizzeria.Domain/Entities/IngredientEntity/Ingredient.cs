namespace Pizzeria.Domain.Entities.IngredientEntity;

public class ProductIngredient
{
    public Guid ProductId { get; private set; }
    public List<Ingredient> Ingredients { get; private set; } = new();


    public ProductIngredient(Guid productId, List<Ingredient> ingredients)
    {
        ProductId = productId;
        Ingredients = ingredients;
    }

    protected ProductIngredient()
    {
    }
}

public class Ingredient
{
    public string Name { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public string Unit { get; private set; } = "g";
    
    public Ingredient(string name, decimal amount, string unit)
    {
        Name = name;
        Amount = amount;
        Unit = unit;
    }
}

