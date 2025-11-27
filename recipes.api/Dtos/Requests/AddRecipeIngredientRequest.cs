namespace Recipes.Api.Dtos.Requests;

public class AddRecipeIngredientRequest
{
    public Guid IngredientId { get; set; }
    public Guid UnitOfMeasurementId { get; set; }
    public int Quantity { get; set; }
}
