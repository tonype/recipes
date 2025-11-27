using Microsoft.EntityFrameworkCore;
using Recipes.Api.Data;
using Recipes.Api.Dtos.Requests;
using Recipes.Api.Dtos.Responses;
using Recipes.Api.Models;
using Recipes.Api.Services.Interfaces;

namespace Recipes.Api.Services.Implementations;

public class UnitOfMeasurementService : IUnitOfMeasurementService
{
    private readonly RecipesDbContext _context;

    public UnitOfMeasurementService(RecipesDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UnitOfMeasurementResponse>> GetAllUnitsAsync()
    {
        return await _context.UnitsOfMeasurement
            .Select(u => new UnitOfMeasurementResponse
            {
                Id = u.Id,
                Name = u.Name,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            })
            .ToListAsync();
    }

    public async Task<UnitOfMeasurementResponse?> GetUnitByIdAsync(Guid id)
    {
        var unit = await _context.UnitsOfMeasurement.FindAsync(id);
        if (unit == null)
            return null;

        return new UnitOfMeasurementResponse
        {
            Id = unit.Id,
            Name = unit.Name,
            CreatedAt = unit.CreatedAt,
            UpdatedAt = unit.UpdatedAt
        };
    }

    public async Task<UnitOfMeasurementResponse> CreateUnitAsync(CreateUnitOfMeasurementRequest request)
    {
        var unit = new UnitOfMeasurement
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        };

        _context.UnitsOfMeasurement.Add(unit);
        await _context.SaveChangesAsync();

        return new UnitOfMeasurementResponse
        {
            Id = unit.Id,
            Name = unit.Name,
            CreatedAt = unit.CreatedAt,
            UpdatedAt = unit.UpdatedAt
        };
    }

    public async Task<bool> DeleteUnitAsync(Guid id)
    {
        var unit = await _context.UnitsOfMeasurement.FindAsync(id);
        if (unit == null)
            return false;

        _context.UnitsOfMeasurement.Remove(unit);
        await _context.SaveChangesAsync();
        return true;
    }
}
