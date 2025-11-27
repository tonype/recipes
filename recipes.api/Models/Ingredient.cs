using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Recipes.Api.Models;

public class Ingredient
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

    // Entity configuration
    public class Configuration : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.ToTable("Ingredients");

            // Primary key
            builder.HasKey(i => i.Id);

            // Properties
            builder.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(i => i.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime2(7)");

            builder.Property(i => i.UpdatedAt)
                .IsRequired()
                .HasColumnType("datetime2(7)");
        }
    }
}
