using Microsoft.AspNetCore.Mvc;
using Recipes.Api.Dtos.Requests;
using Recipes.Api.Services.Interfaces;

namespace Recipes.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientsController : ControllerBase
{
    private readonly IIngredientService _ingredientService;

    public IngredientsController(IIngredientService ingredientService)
    {
        _ingredientService = ingredientService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var ingredients = await _ingredientService.GetAllIngredientsAsync();
        return Ok(ingredients);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var ingredient = await _ingredientService.GetIngredientByIdAsync(id);
        if (ingredient == null)
            return NotFound();

        return Ok(ingredient);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateIngredientRequest request)
    {
        var ingredient = await _ingredientService.CreateIngredientAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = ingredient.Id }, ingredient);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _ingredientService.DeleteIngredientAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}
