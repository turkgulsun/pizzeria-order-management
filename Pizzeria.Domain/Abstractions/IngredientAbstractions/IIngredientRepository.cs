using Pizzeria.Domain.Entities.IngredientEntity;

namespace Pizzeria.Domain.Abstractions.IngredientAbstractions;

public interface IIngredientRepository
{
    Task<IEnumerable<ProductIngredient>> GetAllProductIngredients();
}