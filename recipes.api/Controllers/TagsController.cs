using Microsoft.AspNetCore.Mvc;
using Recipes.Api.Dtos.Requests;
using Recipes.Api.Services.Interfaces;

namespace Recipes.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagsController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagsController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tags = await _tagService.GetAllTagsAsync();
        return Ok(tags);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var tag = await _tagService.GetTagByIdAsync(id);
        if (tag == null)
            return NotFound();

        return Ok(tag);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTagRequest request)
    {
        var tag = await _tagService.CreateTagAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = tag.Id }, tag);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _tagService.DeleteTagAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}
