namespace Recipes.Api.Dtos.Responses;

public class RecipeDetailResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Instructions { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public int PrepTime { get; set; }
    public int CookTime { get; set; }
    public int Difficulty { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<RecipeIngredientDetail> Ingredients { get; set; } = new();
    public List<TagResponse> Tags { get; set; } = new();
    public int TimesMadeCount { get; set; }
}

public class RecipeIngredientDetail
{
    public Guid IngredientId { get; set; }
    public string IngredientName { get; set; } = string.Empty;
    public Guid UnitOfMeasurementId { get; set; }
    public string UnitOfMeasurementName { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
