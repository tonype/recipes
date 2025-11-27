using FluentValidation;
using Recipes.Api.Dtos.Requests;

namespace Recipes.Api.Validators;

public class CreateRecipeValidator : AbstractValidator<CreateRecipeRequest>
{
    public CreateRecipeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(200).WithMessage("Description cannot exceed 200 characters");

        RuleFor(x => x.Instructions)
            .NotEmpty().WithMessage("Instructions are required");

        RuleFor(x => x.Notes)
            .NotEmpty().WithMessage("Notes are required");

        RuleFor(x => x.PrepTime)
            .GreaterThanOrEqualTo(0).WithMessage("Prep time must be 0 or greater");

        RuleFor(x => x.CookTime)
            .GreaterThanOrEqualTo(0).WithMessage("Cook time must be 0 or greater");

        RuleFor(x => x.Difficulty)
            .InclusiveBetween(1, 5).WithMessage("Difficulty must be between 1 and 5");
    }
}
