using Microsoft.EntityFrameworkCore;
using Recipes.Api.Data;
using Recipes.Api.Dtos.Requests;
using Recipes.Api.Dtos.Responses;
using Recipes.Api.Models;
using Recipes.Api.Services.Interfaces;

namespace Recipes.Api.Services.Implementations;

public class TagService : ITagService
{
    private readonly RecipesDbContext _context;

    public TagService(RecipesDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TagResponse>> GetAllTagsAsync()
    {
        return await _context.Tags
            .Select(t => new TagResponse
            {
                Id = t.Id,
                Name = t.Name,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .ToListAsync();
    }

    public async Task<TagResponse?> GetTagByIdAsync(Guid id)
    {
        var tag = await _context.Tags.FindAsync(id);
        if (tag == null)
            return null;

        return new TagResponse
        {
            Id = tag.Id,
            Name = tag.Name,
            CreatedAt = tag.CreatedAt,
            UpdatedAt = tag.UpdatedAt
        };
    }

    public async Task<TagResponse> CreateTagAsync(CreateTagRequest request)
    {
        var tag = new Tag
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        };

        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();

        return new TagResponse
        {
            Id = tag.Id,
            Name = tag.Name,
            CreatedAt = tag.CreatedAt,
            UpdatedAt = tag.UpdatedAt
        };
    }

    public async Task<bool> DeleteTagAsync(Guid id)
    {
        var tag = await _context.Tags.FindAsync(id);
        if (tag == null)
            return false;

        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync();
        return true;
    }
}
