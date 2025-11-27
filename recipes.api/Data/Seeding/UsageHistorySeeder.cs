using Bogus;
using Microsoft.EntityFrameworkCore;
using Recipes.Api.Models;

namespace Recipes.Api.Data.Seeding;

public class UsageHistorySeeder
{
    private readonly RecipesDbContext _context;
    private readonly ILogger<UsageHistorySeeder> _logger;

    public UsageHistorySeeder(RecipesDbContext context, ILogger<UsageHistorySeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> SeedAsync()
    {
        var recipes = await _context.Recipes.ToListAsync();

        if (!recipes.Any())
        {
            _logger.LogWarning("Cannot seed usage history: no recipes found");
            return 0;
        }

        var recipeTimesMade = GenerateRecipeTimesMade(recipes);
        await _context.RecipeTimesMade.AddRangeAsync(recipeTimesMade);
        await _context.SaveChangesAsync();

        _logger.LogDebug("Seeded {Count} recipe usage records", recipeTimesMade.Count);
        return recipeTimesMade.Count;
    }

    private List<RecipeTimeMade> GenerateRecipeTimesMade(List<Recipe> recipes)
    {
        var faker = new Faker();
        var recipeTimesMade = new List<RecipeTimeMade>();
        var startDate = DateTime.UtcNow.AddMonths(-12); // Last 12 months
        var endDate = DateTime.UtcNow;

        foreach (var recipe in recipes)
        {
            // Determine frequency based on distribution:
            // 70% of recipes: 1-5 times
            // 20% of recipes: 6-15 times
            // 10% of recipes: 16-30 times
            var roll = faker.Random.Double(0, 1);
            int timesMade;

            if (roll < 0.70)
            {
                timesMade = faker.Random.Number(1, 5);
            }
            else if (roll < 0.90)
            {
                timesMade = faker.Random.Number(6, 15);
            }
            else
            {
                timesMade = faker.Random.Number(16, 30);
            }

            // Generate unique timestamps for this recipe
            var timestamps = new HashSet<DateTime>();

            while (timestamps.Count < timesMade)
            {
                var madeAt = faker.Date.Between(startDate, endDate);
                // Truncate to minute precision to avoid duplicate issues
                madeAt = new DateTime(madeAt.Year, madeAt.Month, madeAt.Day, madeAt.Hour, madeAt.Minute, 0, DateTimeKind.Utc);

                timestamps.Add(madeAt);
            }

            // Create RecipeTimeMade entries
            foreach (var timestamp in timestamps)
            {
                recipeTimesMade.Add(new RecipeTimeMade
                {
                    RecipeId = recipe.Id,
                    MadeAt = timestamp
                });
            }
        }

        return recipeTimesMade;
    }
}
