using Recipes.Api.Dtos.Requests;
using Recipes.Api.Dtos.Responses;

namespace Recipes.Api.Services.Interfaces;

public interface IIngredientService
{
    Task<IEnumerable<IngredientResponse>> GetAllIngredientsAsync();
    Task<IngredientResponse?> GetIngredientByIdAsync(Guid id);
    Task<IngredientResponse> CreateIngredientAsync(CreateIngredientRequest request);
    Task<bool> DeleteIngredientAsync(Guid id);
}
