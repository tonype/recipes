using Microsoft.EntityFrameworkCore;
using Recipes.Api.Data;
using Recipes.Api.Dtos.Requests;
using Recipes.Api.Dtos.Responses;
using Recipes.Api.Models;
using Recipes.Api.Services.Interfaces;

namespace Recipes.Api.Services.Implementations;

public class RecipeService : IRecipeService
{
    private readonly RecipesDbContext _context;

    public RecipeService(RecipesDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RecipeResponse>> GetAllRecipesAsync()
    {
        return await _context.Recipes
            .Select(r => new RecipeResponse
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                Instructions = r.Instructions,
                Notes = r.Notes,
                PrepTime = r.PrepTime,
                CookTime = r.CookTime,
                Difficulty = r.Difficulty,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            })
            .ToListAsync();
    }

    public async Task<RecipeDetailResponse?> GetRecipeByIdAsync(Guid id)
    {
        var recipe = await _context.Recipes
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.UnitOfMeasurement)
            .Include(r => r.RecipeTags)
                .ThenInclude(rt => rt.Tag)
            .Include(r => r.RecipeTimesMade)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (recipe == null)
            return null;

        return new RecipeDetailResponse
        {
            Id = recipe.Id,
            Name = recipe.Name,
            Description = recipe.Description,
            Instructions = recipe.Instructions,
            Notes = recipe.Notes,
            PrepTime = recipe.PrepTime,
            CookTime = recipe.CookTime,
            Difficulty = recipe.Difficulty,
            CreatedAt = recipe.CreatedAt,
            UpdatedAt = recipe.UpdatedAt,
            Ingredients = recipe.RecipeIngredients.Select(ri => new RecipeIngredientDetail
            {
                IngredientId = ri.IngredientId,
                IngredientName = ri.Ingredient.Name,
                UnitOfMeasurementId = ri.UnitOfMeasurementId,
                UnitOfMeasurementName = ri.UnitOfMeasurement.Name,
                Quantity = ri.Quantity
            }).ToList(),
            Tags = recipe.RecipeTags.Select(rt => new TagResponse
            {
                Id = rt.TagId,
                Name = rt.Tag.Name,
                CreatedAt = rt.Tag.CreatedAt,
                UpdatedAt = rt.Tag.UpdatedAt
            }).ToList(),
            TimesMadeCount = recipe.RecipeTimesMade.Count
        };
    }

    public async Task<RecipeResponse> CreateRecipeAsync(CreateRecipeRequest request)
    {
        var recipe = new Recipe
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Instructions = request.Instructions,
            Notes = request.Notes,
            PrepTime = request.PrepTime,
            CookTime = request.CookTime,
            Difficulty = request.Difficulty
        };

        _context.Recipes.Add(recipe);
        await _context.SaveChangesAsync();

        return new RecipeResponse
        {
            Id = recipe.Id,
            Name = recipe.Name,
            Description = recipe.Description,
            Instructions = recipe.Instructions,
            Notes = recipe.Notes,
            PrepTime = recipe.PrepTime,
            CookTime = recipe.CookTime,
            Difficulty = recipe.Difficulty,
            CreatedAt = recipe.CreatedAt,
            UpdatedAt = recipe.UpdatedAt
        };
    }

    public async Task<RecipeResponse?> UpdateRecipeAsync(Guid id, UpdateRecipeRequest request)
    {
        var recipe = await _context.Recipes.FindAsync(id);
        if (recipe == null)
            return null;

        recipe.Name = request.Name;
        recipe.Description = request.Description;
        recipe.Instructions = request.Instructions;
        recipe.Notes = request.Notes;
        recipe.PrepTime = request.PrepTime;
        recipe.CookTime = request.CookTime;
        recipe.Difficulty = request.Difficulty;

        await _context.SaveChangesAsync();

        return new RecipeResponse
        {
            Id = recipe.Id,
            Name = recipe.Name,
            Description = recipe.Description,
            Instructions = recipe.Instructions,
            Notes = recipe.Notes,
            PrepTime = recipe.PrepTime,
            CookTime = recipe.CookTime,
            Difficulty = recipe.Difficulty,
            CreatedAt = recipe.CreatedAt,
            UpdatedAt = recipe.UpdatedAt
        };
    }

    public async Task<bool> DeleteRecipeAsync(Guid id)
    {
        var recipe = await _context.Recipes.FindAsync(id);
        if (recipe == null)
            return false;

        _context.Recipes.Remove(recipe);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddIngredientToRecipeAsync(Guid recipeId, AddRecipeIngredientRequest request)
    {
        var recipe = await _context.Recipes.FindAsync(recipeId);
        if (recipe == null)
            return false;

        var ingredient = await _context.Ingredients.FindAsync(request.IngredientId);
        if (ingredient == null)
            return false;

        var unit = await _context.UnitsOfMeasurement.FindAsync(request.UnitOfMeasurementId);
        if (unit == null)
            return false;

        var recipeIngredient = new RecipeIngredient
        {
            RecipeId = recipeId,
            IngredientId = request.IngredientId,
            UnitOfMeasurementId = request.UnitOfMeasurementId,
            Quantity = request.Quantity
        };

        _context.RecipeIngredients.Add(recipeIngredient);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddTagToRecipeAsync(Guid recipeId, Guid tagId)
    {
        var recipe = await _context.Recipes.FindAsync(recipeId);
        if (recipe == null)
            return false;

        var tag = await _context.Tags.FindAsync(tagId);
        if (tag == null)
            return false;

        var recipeTag = new RecipeTag
        {
            RecipeId = recipeId,
            TagId = tagId
        };

        _context.RecipeTags.Add(recipeTag);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RecordRecipeTimeMadeAsync(Guid recipeId)
    {
        var recipe = await _context.Recipes.FindAsync(recipeId);
        if (recipe == null)
            return false;

        var recipeTimeMade = new RecipeTimeMade
        {
            RecipeId = recipeId,
            MadeAt = DateTime.UtcNow
        };

        _context.RecipeTimesMade.Add(recipeTimeMade);
        await _context.SaveChangesAsync();
        return true;
    }
}
