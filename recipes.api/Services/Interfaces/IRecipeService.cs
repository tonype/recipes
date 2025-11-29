using Recipes.Api.Dtos.Requests;
using Recipes.Api.Dtos.Responses;

namespace Recipes.Api.Services.Interfaces;

public interface IRecipeService
{
    Task<IEnumerable<RecipeResponse>> GetAllRecipesAsync();
    Task<PagedResponse<RecipeResponse>> GetRecipesAsync(RecipeQueryParameters parameters);
    Task<RecipeDetailResponse?> GetRecipeByIdAsync(Guid id);
    Task<RecipeResponse> CreateRecipeAsync(CreateRecipeRequest request);
    Task<RecipeResponse?> UpdateRecipeAsync(Guid id, UpdateRecipeRequest request);
    Task<bool> DeleteRecipeAsync(Guid id);
    Task<bool> AddIngredientToRecipeAsync(Guid recipeId, AddRecipeIngredientRequest request);
    Task<bool> AddTagToRecipeAsync(Guid recipeId, Guid tagId);
    Task<bool> RecordRecipeTimeMadeAsync(Guid recipeId);
}
