using FluentValidation;
using Recipes.Api.Dtos.Requests;

namespace Recipes.Api.Validators;

public class CreateTagValidator : AbstractValidator<CreateTagRequest>
{
    public CreateTagValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");
    }
}
