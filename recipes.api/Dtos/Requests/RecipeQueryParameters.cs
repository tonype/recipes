namespace Recipes.Api.Dtos.Requests;

public class RecipeQueryParameters : PaginationRequest
{
    public string? SortBy { get; set; }
    public string? SortOrder { get; set; } = "asc";
}
