namespace Recipes.Api.Dtos.Requests;

public class UpdateRecipeRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Instructions { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public int PrepTime { get; set; }
    public int CookTime { get; set; }
    public int Difficulty { get; set; }
}
