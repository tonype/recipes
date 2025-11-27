using Bogus;
using Microsoft.EntityFrameworkCore;
using Recipes.Api.Models;

namespace Recipes.Api.Data.Seeding;

public class RecipeSeeder
{
    private readonly RecipesDbContext _context;
    private readonly ILogger<RecipeSeeder> _logger;

    public RecipeSeeder(RecipesDbContext context, ILogger<RecipeSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> SeedAsync()
    {
        // Load existing master data
        var ingredients = await _context.Ingredients.ToListAsync();
        var tags = await _context.Tags.ToListAsync();
        var units = await _context.UnitsOfMeasurement.ToListAsync();

        if (!ingredients.Any() || !tags.Any() || !units.Any())
        {
            _logger.LogWarning("Cannot seed recipes: master data is missing");
            return 0;
        }

        var recipeCount = 75; // Generate 75 recipes
        var recipes = GenerateRecipes(recipeCount);

        await _context.Recipes.AddRangeAsync(recipes);
        await _context.SaveChangesAsync();

        var totalSeeded = recipes.Count;

        // Generate RecipeIngredients relationships
        var recipeIngredients = GenerateRecipeIngredients(recipes, ingredients, units);
        await _context.RecipeIngredients.AddRangeAsync(recipeIngredients);
        await _context.SaveChangesAsync();
        totalSeeded += recipeIngredients.Count;
        _logger.LogDebug("Seeded {Count} recipe-ingredient relationships", recipeIngredients.Count);

        // Generate RecipeTags relationships
        var recipeTags = GenerateRecipeTags(recipes, tags);
        await _context.RecipeTags.AddRangeAsync(recipeTags);
        await _context.SaveChangesAsync();
        totalSeeded += recipeTags.Count;
        _logger.LogDebug("Seeded {Count} recipe-tag relationships", recipeTags.Count);

        _logger.LogDebug("Seeded {Count} recipes", recipes.Count);

        return totalSeeded;
    }

    private List<Recipe> GenerateRecipes(int count)
    {
        var dishTypes = new[] { "Pasta", "Soup", "Salad", "Stir-Fry", "Casserole", "Curry", "Tacos", "Pizza", "Burger", "Sandwich", "Bowl", "Skillet" };
        var proteins = new[] { "Chicken", "Beef", "Pork", "Shrimp", "Salmon", "Tofu", "Turkey", "Lamb" };
        var flavors = new[] { "Spicy", "Garlic", "Lemon", "Herb", "Creamy", "Tangy", "Sweet", "Savory", "BBQ", "Teriyaki", "Mediterranean", "Asian-Style" };

        var faker = new Faker();
        var recipes = new List<Recipe>();

        for (int i = 0; i < count; i++)
        {
            var dishType = faker.PickRandom(dishTypes);
            var protein = faker.PickRandom(proteins);
            var flavor = faker.PickRandom(flavors);

            var recipeName = faker.Random.Bool(0.6f)
                ? $"{flavor} {protein} {dishType}"
                : $"{protein} {dishType}";

            recipes.Add(new Recipe
            {
                Id = Guid.NewGuid(),
                Name = recipeName,
                Description = GenerateDescription(recipeName, faker),
                Instructions = GenerateInstructions(faker),
                Notes = faker.Lorem.Paragraph(2),
                PrepTime = faker.Random.Number(5, 60),
                CookTime = faker.Random.Number(10, 120),
                Difficulty = faker.Random.Number(1, 5)
            });
        }

        return recipes;
    }

    private string GenerateDescription(string recipeName, Faker faker)
    {
        var templates = new[]
        {
            $"A delicious {recipeName.ToLower()} that's perfect for any occasion. {faker.Lorem.Sentence(8)}",
            $"This {recipeName.ToLower()} is a family favorite. {faker.Lorem.Sentence(10)}",
            $"Quick and easy {recipeName.ToLower()} recipe. {faker.Lorem.Sentence(9)}",
            $"Restaurant-quality {recipeName.ToLower()} made at home. {faker.Lorem.Sentence(7)}"
        };

        return faker.PickRandom(templates);
    }

    private string GenerateInstructions(Faker faker)
    {
        var steps = faker.Random.Number(4, 8);
        var instructions = new List<string>();

        for (int i = 1; i <= steps; i++)
        {
            instructions.Add($"{i}. {faker.Lorem.Sentence(12)}");
        }

        return string.Join("\n", instructions);
    }

    private List<RecipeIngredient> GenerateRecipeIngredients(
        List<Recipe> recipes,
        List<Ingredient> ingredients,
        List<UnitOfMeasurement> units)
    {
        var faker = new Faker();
        var recipeIngredients = new List<RecipeIngredient>();

        foreach (var recipe in recipes)
        {
            // Each recipe gets 3-10 ingredients
            var ingredientCount = faker.Random.Number(3, 10);
            var selectedIngredients = faker.PickRandom(ingredients, ingredientCount);

            foreach (var ingredient in selectedIngredients)
            {
                var unit = faker.PickRandom(units);

                recipeIngredients.Add(new RecipeIngredient
                {
                    RecipeId = recipe.Id,
                    IngredientId = ingredient.Id,
                    UnitOfMeasurementId = unit.Id,
                    Quantity = faker.Random.Number(1, 10)
                });
            }
        }

        return recipeIngredients;
    }

    private List<RecipeTag> GenerateRecipeTags(List<Recipe> recipes, List<Tag> tags)
    {
        var faker = new Faker();
        var recipeTags = new List<RecipeTag>();

        foreach (var recipe in recipes)
        {
            // Each recipe gets 2-5 tags
            var tagCount = faker.Random.Number(2, 5);
            var selectedTags = faker.PickRandom(tags, tagCount);

            foreach (var tag in selectedTags)
            {
                recipeTags.Add(new RecipeTag
                {
                    RecipeId = recipe.Id,
                    TagId = tag.Id
                });
            }
        }

        return recipeTags;
    }
}
