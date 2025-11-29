using Microsoft.AspNetCore.Mvc;
using Recipes.Api.Dtos.Requests;
using Recipes.Api.Services.Interfaces;

namespace Recipes.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipesController : ControllerBase
{
    private readonly IRecipeService _recipeService;

    public RecipesController(IRecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] RecipeQueryParameters parameters)
    {
        var pagedRecipes = await _recipeService.GetRecipesAsync(parameters);
        return Ok(pagedRecipes);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var recipe = await _recipeService.GetRecipeByIdAsync(id);
        if (recipe == null)
            return NotFound();

        return Ok(recipe);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRecipeRequest request)
    {
        var recipe = await _recipeService.CreateRecipeAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = recipe.Id }, recipe);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRecipeRequest request)
    {
        var recipe = await _recipeService.UpdateRecipeAsync(id, request);
        if (recipe == null)
            return NotFound();

        return Ok(recipe);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _recipeService.DeleteRecipeAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpPost("{id:guid}/ingredients")]
    public async Task<IActionResult> AddIngredient(Guid id, [FromBody] AddRecipeIngredientRequest request)
    {
        var result = await _recipeService.AddIngredientToRecipeAsync(id, request);
        if (!result)
            return NotFound();

        return Ok();
    }

    [HttpPost("{id:guid}/tags/{tagId:guid}")]
    public async Task<IActionResult> AddTag(Guid id, Guid tagId)
    {
        var result = await _recipeService.AddTagToRecipeAsync(id, tagId);
        if (!result)
            return NotFound();

        return Ok();
    }

    [HttpPost("{id:guid}/times-made")]
    public async Task<IActionResult> RecordTimeMade(Guid id)
    {
        var result = await _recipeService.RecordRecipeTimeMadeAsync(id);
        if (!result)
            return NotFound();

        return Ok();
    }
}
