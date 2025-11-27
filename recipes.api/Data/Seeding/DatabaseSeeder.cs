using Microsoft.EntityFrameworkCore;
using Recipes.Api.Models;

namespace Recipes.Api.Data.Seeding;

public class DatabaseSeeder
{
    private readonly RecipesDbContext _context;
    private readonly MasterDataSeeder _masterDataSeeder;
    private readonly RecipeSeeder _recipeSeeder;
    private readonly UsageHistorySeeder _usageHistorySeeder;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(
        RecipesDbContext context,
        MasterDataSeeder masterDataSeeder,
        RecipeSeeder recipeSeeder,
        UsageHistorySeeder usageHistorySeeder,
        ILogger<DatabaseSeeder> logger)
    {
        _context = context;
        _masterDataSeeder = masterDataSeeder;
        _recipeSeeder = recipeSeeder;
        _usageHistorySeeder = usageHistorySeeder;
        _logger = logger;
    }

    public async Task<SeedResult> SeedAllAsync(bool clearExisting = false)
    {
        try
        {
            _logger.LogInformation("Starting database seeding (clearExisting: {ClearExisting})", clearExisting);

            // Check if data already exists
            if (!clearExisting)
            {
                var hasRecipes = await _context.Recipes.AnyAsync();
                var hasIngredients = await _context.Ingredients.AnyAsync();

                if (hasRecipes || hasIngredients)
                {
                    _logger.LogWarning("Database already contains data. Use clearExisting=true to re-seed.");
                    return new SeedResult
                    {
                        Success = false,
                        ErrorMessage = "Database already contains data. Use clearExisting=true to re-seed."
                    };
                }
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Clear existing data if requested (but NOT UnitsOfMeasurement - they're from migration)
                if (clearExisting)
                {
                    _logger.LogInformation("Clearing existing development data...");
                    await ClearAllDataAsync();
                }

                // Seed in correct order
                _logger.LogInformation("Seeding master data (Ingredients, Tags)...");
                var masterDataCount = await _masterDataSeeder.SeedAsync();

                _logger.LogInformation("Seeding recipes with relationships...");
                var recipeCount = await _recipeSeeder.SeedAsync();

                _logger.LogInformation("Seeding usage history...");
                var historyCount = await _usageHistorySeeder.SeedAsync();

                await transaction.CommitAsync();

                var totalCount = masterDataCount + recipeCount + historyCount;
                _logger.LogInformation("Database seeding completed successfully. Total items: {TotalCount}", totalCount);

                return new SeedResult
                {
                    Success = true,
                    ItemsSeeded = totalCount,
                    Details = new Dictionary<string, int>
                    {
                        { "MasterData", masterDataCount },
                        { "Recipes", recipeCount },
                        { "UsageHistory", historyCount }
                    }
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during database seeding");
            return new SeedResult
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    private async Task ClearAllDataAsync()
    {
        // Clear in reverse dependency order
        // IMPORTANT: Do NOT clear UnitsOfMeasurement - they're from migration
        _context.RecipeTimesMade.RemoveRange(_context.RecipeTimesMade);
        _context.RecipeTags.RemoveRange(_context.RecipeTags);
        _context.RecipeIngredients.RemoveRange(_context.RecipeIngredients);
        _context.Recipes.RemoveRange(_context.Recipes);
        _context.Tags.RemoveRange(_context.Tags);
        _context.Ingredients.RemoveRange(_context.Ingredients);

        await _context.SaveChangesAsync();
        _logger.LogInformation("Cleared existing development data (preserved UnitsOfMeasurement)");
    }

    public async Task<DatabaseStatus> GetDatabaseStatusAsync()
    {
        return new DatabaseStatus
        {
            UnitsOfMeasurement = await _context.UnitsOfMeasurement.CountAsync(),
            Ingredients = await _context.Ingredients.CountAsync(),
            Tags = await _context.Tags.CountAsync(),
            Recipes = await _context.Recipes.CountAsync(),
            RecipeIngredients = await _context.RecipeIngredients.CountAsync(),
            RecipeTags = await _context.RecipeTags.CountAsync(),
            RecipeTimesMade = await _context.RecipeTimesMade.CountAsync()
        };
    }
}

public class SeedResult
{
    public bool Success { get; set; }
    public int ItemsSeeded { get; set; }
    public Dictionary<string, int>? Details { get; set; }
    public string? ErrorMessage { get; set; }
}

public class DatabaseStatus
{
    public int UnitsOfMeasurement { get; set; }
    public int Ingredients { get; set; }
    public int Tags { get; set; }
    public int Recipes { get; set; }
    public int RecipeIngredients { get; set; }
    public int RecipeTags { get; set; }
    public int RecipeTimesMade { get; set; }
}
