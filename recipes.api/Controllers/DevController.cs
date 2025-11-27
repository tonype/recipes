#if DEBUG
using Microsoft.AspNetCore.Mvc;
using Recipes.Api.Data.Seeding;

namespace Recipes.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevController : ControllerBase
{
    private readonly DatabaseSeeder _seeder;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<DevController> _logger;

    public DevController(
        DatabaseSeeder seeder,
        IWebHostEnvironment env,
        ILogger<DevController> logger)
    {
        _seeder = seeder;
        _env = env;
        _logger = logger;
    }

    [HttpPost("seed")]
    public async Task<IActionResult> SeedDatabase([FromQuery] bool clearExisting = false)
    {
        // Double-check environment
        if (!_env.IsDevelopment())
        {
            _logger.LogWarning("Seed endpoint called in non-Development environment");
            return StatusCode(403, new { error = "This endpoint is only available in Development" });
        }

        try
        {
            _logger.LogInformation("Starting database seeding via API (clearExisting: {Clear})", clearExisting);
            var result = await _seeder.SeedAllAsync(clearExisting);

            if (!result.Success)
            {
                return BadRequest(new { error = result.ErrorMessage });
            }

            _logger.LogInformation("Seeding completed: {Count} items", result.ItemsSeeded);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during database seeding");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("seed/status")]
    public async Task<IActionResult> GetDatabaseStatus()
    {
        if (!_env.IsDevelopment())
        {
            return StatusCode(403, new { error = "This endpoint is only available in Development" });
        }

        try
        {
            var status = await _seeder.GetDatabaseStatusAsync();
            return Ok(status);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting database status");
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
#endif
