using FluentValidation;
using Recipes.Api.Dtos.Requests;

namespace Recipes.Api.Validators;

public class AddRecipeIngredientValidator : AbstractValidator<AddRecipeIngredientRequest>
{
    public AddRecipeIngredientValidator()
    {
        RuleFor(x => x.IngredientId)
            .NotEmpty().WithMessage("Ingredient ID is required");

        RuleFor(x => x.UnitOfMeasurementId)
            .NotEmpty().WithMessage("Unit of Measurement ID is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}
