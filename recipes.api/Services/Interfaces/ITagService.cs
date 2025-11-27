using Recipes.Api.Dtos.Requests;
using Recipes.Api.Dtos.Responses;

namespace Recipes.Api.Services.Interfaces;

public interface ITagService
{
    Task<IEnumerable<TagResponse>> GetAllTagsAsync();
    Task<TagResponse?> GetTagByIdAsync(Guid id);
    Task<TagResponse> CreateTagAsync(CreateTagRequest request);
    Task<bool> DeleteTagAsync(Guid id);
}
