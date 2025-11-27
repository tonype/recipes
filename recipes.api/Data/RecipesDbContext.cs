using Microsoft.EntityFrameworkCore;
using Recipes.Api.Models;

namespace Recipes.Api.Data;

public class RecipesDbContext : DbContext
{
    public RecipesDbContext(DbContextOptions<RecipesDbContext> options) : base(options) { }

    // Main entities
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<UnitOfMeasurement> UnitsOfMeasurement => Set<UnitOfMeasurement>();
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();

    // Junction/tracking tables
    public DbSet<RecipeIngredient> RecipeIngredients => Set<RecipeIngredient>();
    public DbSet<RecipeTag> RecipeTags => Set<RecipeTag>();
    public DbSet<RecipeTimeMade> RecipeTimesMade => Set<RecipeTimeMade>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations from Configurations folder
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RecipesDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Automatically set CreatedAt and UpdatedAt timestamps
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                if (entry.Property("CreatedAt").CurrentValue == null ||
                    (DateTime)entry.Property("CreatedAt").CurrentValue == default)
                {
                    entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                }
            }

            if (entry.Property("UpdatedAt") != null)
            {
                entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
