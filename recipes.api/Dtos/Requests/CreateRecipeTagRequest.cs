namespace Recipes.Api.Dtos.Requests;

public class CreateRecipeTagRequest
{
    public Guid? TagId { get; set; }      // For existing tags (has ID)
    public string? TagName { get; set; }   // For new tags (no ID yet)
}