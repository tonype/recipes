using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Recipes.Api.Models;

public class Recipe
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Instructions { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public int PrepTime { get; set; }
    public int CookTime { get; set; }
    public int Difficulty { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    public ICollection<RecipeTag> RecipeTags { get; set; } = new List<RecipeTag>();
    public ICollection<RecipeTimeMade> RecipeTimesMade { get; set; } = new List<RecipeTimeMade>();

    // Entity configuration
    public class Configuration : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.ToTable("Recipes");

            // Primary key
            builder.HasKey(r => r.Id);

            // Properties
            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.Description)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(r => r.Instructions)
                .IsRequired();

            builder.Property(r => r.Notes)
                .IsRequired(false);

            builder.Property(r => r.PrepTime)
                .IsRequired();

            builder.Property(r => r.CookTime)
                .IsRequired();

            builder.Property(r => r.Difficulty)
                .IsRequired();

            builder.Property(r => r.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime2(7)");

            builder.Property(r => r.UpdatedAt)
                .IsRequired()
                .HasColumnType("datetime2(7)");

            // Relationships are configured in the junction table configurations
        }
    }
}
