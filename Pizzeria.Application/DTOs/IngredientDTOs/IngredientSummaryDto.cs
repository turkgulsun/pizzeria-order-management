namespace Pizzeria.Application.DTOs.IngredientDTOs;

public record IngredientSummaryDto(
    string IngredientName,
    decimal TotalAmount,
    string Unit);