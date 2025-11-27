using Recipes.Api.Dtos.Requests;
using Recipes.Api.Dtos.Responses;

namespace Recipes.Api.Services.Interfaces;

public interface IUnitOfMeasurementService
{
    Task<IEnumerable<UnitOfMeasurementResponse>> GetAllUnitsAsync();
    Task<UnitOfMeasurementResponse?> GetUnitByIdAsync(Guid id);
    Task<UnitOfMeasurementResponse> CreateUnitAsync(CreateUnitOfMeasurementRequest request);
    Task<bool> DeleteUnitAsync(Guid id);
}
