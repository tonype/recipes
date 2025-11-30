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

    public async Task<PagedResponse<RecipeResponse>> GetRecipesAsync(RecipeQueryParameters parameters)
    {
        IQueryable<Recipe> query = _context.Recipes;

        query = ApplySorting(query, parameters.SortBy, parameters.SortOrder);

        var totalCount = await query.CountAsync();

        var recipes = await query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
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

        return new PagedResponse<RecipeResponse>(
            recipes,
            totalCount,
            parameters.PageNumber,
            parameters.PageSize
        );
    }

    private static IQueryable<Recipe> ApplySorting(
        IQueryable<Recipe> query,
        string? sortBy,
        string? sortOrder)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
        {
            return query.OrderByDescending(r => r.CreatedAt);
        }

        var isDescending = sortOrder?.ToLower() == "desc";

        return sortBy.ToLower() switch
        {
            "name" => isDescending
                ? query.OrderByDescending(r => r.Name)
                : query.OrderBy(r => r.Name),
            "createdat" => isDescending
                ? query.OrderByDescending(r => r.CreatedAt)
                : query.OrderBy(r => r.CreatedAt),
            "preptime" => isDescending
                ? query.OrderByDescending(r => r.PrepTime)
                : query.OrderBy(r => r.PrepTime),
            "cooktime" => isDescending
                ? query.OrderByDescending(r => r.CookTime)
                : query.OrderBy(r => r.CookTime),
            "difficulty" => isDescending
                ? query.OrderByDescending(r => r.Difficulty)
                : query.OrderBy(r => r.Difficulty),
            _ => query.OrderByDescending(r => r.CreatedAt)
        };
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
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
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

            // Process tags if any are provided
            if (request.Tags?.Any() == true)
            {
                foreach (var tagRequest in request.Tags.Where(t => t != null))
                {
                    Tag tag;

                    if (tagRequest.TagId.HasValue)
                    {
                        // Existing tag - use ID directly
                        tag = await _context.Tags.FindAsync(tagRequest.TagId.Value);
                        if (tag == null)
                            throw new ArgumentException($"Tag with ID {tagRequest.TagId} not found");
                    }
                    else if (!string.IsNullOrWhiteSpace(tagRequest.TagName))
                    {
                        // New tag - check if name exists, create if not
                        var trimmedTagName = tagRequest.TagName.Trim();
                        var existingTag = await _context.Tags
                            .FirstOrDefaultAsync(t => t.Name.ToLower() == trimmedTagName.ToLower());

                        if (existingTag != null)
                        {
                            tag = existingTag;
                        }
                        else
                        {
                            tag = new Tag
                            {
                                Id = Guid.NewGuid(),
                                Name = trimmedTagName
                            };
                            _context.Tags.Add(tag);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        continue; // Skip invalid tag requests
                    }

                    // Create recipe-tag association using ID
                    var recipeTag = new RecipeTag
                    {
                        RecipeId = recipe.Id,
                        TagId = tag.Id
                    };
                    _context.RecipeTags.Add(recipeTag);
                }

                await _context.SaveChangesAsync();
            }

            await transaction.CommitAsync();

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
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
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
