using Bogus;
using Recipes.Api.Models;

namespace Recipes.Api.Data.Seeding;

public class MasterDataSeeder
{
    private readonly RecipesDbContext _context;
    private readonly ILogger<MasterDataSeeder> _logger;

    public MasterDataSeeder(RecipesDbContext context, ILogger<MasterDataSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> SeedAsync()
    {
        var totalSeeded = 0;

        // Seed Ingredients
        var ingredients = GenerateIngredients(150);
        await _context.Ingredients.AddRangeAsync(ingredients);
        await _context.SaveChangesAsync();
        totalSeeded += ingredients.Count;
        _logger.LogDebug("Seeded {Count} ingredients", ingredients.Count);

        // Seed Tags
        var tags = GenerateTags();
        await _context.Tags.AddRangeAsync(tags);
        await _context.SaveChangesAsync();
        totalSeeded += tags.Count;
        _logger.LogDebug("Seeded {Count} tags", tags.Count);

        return totalSeeded;
    }

    private List<Ingredient> GenerateIngredients(int count)
    {
        var categories = new Dictionary<string, List<string>>
        {
            { "Produce", new List<string> { "tomato", "lettuce", "onion", "garlic", "potato", "carrot", "celery", "bell pepper", "cucumber", "zucchini", "spinach", "kale", "broccoli", "cauliflower", "mushroom", "avocado", "lemon", "lime", "apple", "banana" } },
            { "Dairy", new List<string> { "milk", "butter", "cream", "yogurt", "sour cream", "cheddar cheese", "mozzarella cheese", "parmesan cheese", "feta cheese", "cream cheese", "ricotta cheese", "goat cheese" } },
            { "Meat", new List<string> { "chicken breast", "chicken thigh", "ground beef", "beef steak", "pork chop", "bacon", "sausage", "ham", "lamb", "turkey", "ground turkey" } },
            { "Seafood", new List<string> { "salmon", "tuna", "shrimp", "cod", "tilapia", "crab", "lobster", "mussels", "clams", "scallops" } },
            { "Grains", new List<string> { "rice", "pasta", "flour", "bread", "quinoa", "couscous", "oats", "cornmeal", "breadcrumbs", "tortilla" } },
            { "Spices", new List<string> { "salt", "black pepper", "paprika", "cumin", "oregano", "basil", "thyme", "rosemary", "cinnamon", "nutmeg", "ginger", "turmeric", "chili powder", "cayenne pepper", "red pepper flakes", "bay leaf", "parsley", "cilantro", "dill", "sage" } },
            { "Condiments", new List<string> { "olive oil", "vegetable oil", "soy sauce", "vinegar", "balsamic vinegar", "mustard", "ketchup", "mayonnaise", "hot sauce", "worcestershire sauce", "fish sauce", "sesame oil", "honey", "maple syrup", "sugar", "brown sugar" } },
            { "Baking", new List<string> { "baking powder", "baking soda", "vanilla extract", "cocoa powder", "chocolate chips", "powdered sugar", "cornstarch", "yeast" } },
            { "Canned", new List<string> { "tomato paste", "tomato sauce", "diced tomatoes", "chicken broth", "beef broth", "vegetable broth", "coconut milk", "beans", "chickpeas", "corn" } },
            { "Nuts", new List<string> { "almonds", "walnuts", "pecans", "peanuts", "cashews", "pine nuts", "pistachios" } }
        };

        var ingredients = new List<Ingredient>();
        var faker = new Faker();

        foreach (var category in categories)
        {
            foreach (var ingredientName in category.Value)
            {
                ingredients.Add(new Ingredient
                {
                    Id = Guid.NewGuid(),
                    Name = ingredientName
                });

                if (ingredients.Count >= count)
                    break;
            }

            if (ingredients.Count >= count)
                break;
        }

        // If we need more ingredients, generate some random ones
        while (ingredients.Count < count)
        {
            var randomName = faker.Commerce.ProductName().ToLower();
            if (!ingredients.Any(i => i.Name == randomName))
            {
                ingredients.Add(new Ingredient
                {
                    Id = Guid.NewGuid(),
                    Name = randomName
                });
            }
        }

        return ingredients;
    }

    private List<Tag> GenerateTags()
    {
        var tagNames = new List<string>
        {
            // Cuisine types
            "Italian", "Mexican", "Chinese", "Japanese", "Thai", "Indian", "French", "Greek", "Mediterranean", "American", "Korean", "Vietnamese",

            // Meal types
            "Breakfast", "Lunch", "Dinner", "Appetizer", "Dessert", "Snack", "Soup", "Salad", "Side Dish",

            // Dietary
            "Vegetarian", "Vegan", "Gluten-Free", "Dairy-Free", "Low-Carb", "Keto", "Paleo", "Healthy",

            // Cooking methods
            "Grilled", "Baked", "Fried", "Slow Cooker", "Instant Pot", "One Pot", "No-Bake",

            // Occasions
            "Holiday", "Party", "Kid-Friendly", "Quick & Easy", "Comfort Food"
        };

        return tagNames.Select(name => new Tag
        {
            Id = Guid.NewGuid(),
            Name = name
        }).ToList();
    }
}
