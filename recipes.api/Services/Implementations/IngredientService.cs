using Microsoft.EntityFrameworkCore;
using Recipes.Api.Data;
using Recipes.Api.Dtos.Requests;
using Recipes.Api.Dtos.Responses;
using Recipes.Api.Models;
using Recipes.Api.Services.Interfaces;

namespace Recipes.Api.Services.Implementations;

public class IngredientService : IIngredientService
{
    private readonly RecipesDbContext _context;

    public IngredientService(RecipesDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<IngredientResponse>> GetAllIngredientsAsync()
    {
        return await _context.Ingredients
            .Select(i => new IngredientResponse
            {
                Id = i.Id,
                Name = i.Name,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt
            })
            .ToListAsync();
    }

    public async Task<IngredientResponse?> GetIngredientByIdAsync(Guid id)
    {
        var ingredient = await _context.Ingredients.FindAsync(id);
        if (ingredient == null)
            return null;

        return new IngredientResponse
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            CreatedAt = ingredient.CreatedAt,
            UpdatedAt = ingredient.UpdatedAt
        };
    }

    public async Task<IngredientResponse> CreateIngredientAsync(CreateIngredientRequest request)
    {
        var ingredient = new Ingredient
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        };

        _context.Ingredients.Add(ingredient);
        await _context.SaveChangesAsync();

        return new IngredientResponse
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            CreatedAt = ingredient.CreatedAt,
            UpdatedAt = ingredient.UpdatedAt
        };
    }

    public async Task<bool> DeleteIngredientAsync(Guid id)
    {
        var ingredient = await _context.Ingredients.FindAsync(id);
        if (ingredient == null)
            return false;

        _context.Ingredients.Remove(ingredient);
        await _context.SaveChangesAsync();
        return true;
    }
}
