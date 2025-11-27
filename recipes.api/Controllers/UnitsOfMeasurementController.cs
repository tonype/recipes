using Microsoft.AspNetCore.Mvc;
using Recipes.Api.Dtos.Requests;
using Recipes.Api.Services.Interfaces;

namespace Recipes.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UnitsOfMeasurementController : ControllerBase
{
    private readonly IUnitOfMeasurementService _unitService;

    public UnitsOfMeasurementController(IUnitOfMeasurementService unitService)
    {
        _unitService = unitService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var units = await _unitService.GetAllUnitsAsync();
        return Ok(units);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var unit = await _unitService.GetUnitByIdAsync(id);
        if (unit == null)
            return NotFound();

        return Ok(unit);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUnitOfMeasurementRequest request)
    {
        var unit = await _unitService.CreateUnitAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = unit.Id }, unit);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _unitService.DeleteUnitAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}
