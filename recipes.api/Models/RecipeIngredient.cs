using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Recipes.Api.Models;

public class RecipeIngredient
{
    public Guid RecipeId { get; set; }
    public Guid IngredientId { get; set; }
    public Guid UnitOfMeasurementId { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public Recipe Recipe { get; set; } = null!;
    public Ingredient Ingredient { get; set; } = null!;
    public UnitOfMeasurement UnitOfMeasurement { get; set; } = null!;

    // Entity configuration
    public class Configuration : IEntityTypeConfiguration<RecipeIngredient>
    {
        public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
        {
            builder.ToTable("RecipeIngredients");

            // Composite primary key
            builder.HasKey(ri => new { ri.RecipeId, ri.IngredientId, ri.UnitOfMeasurementId });

            // Properties
            builder.Property(ri => ri.Quantity)
                .IsRequired();

            builder.Property(ri => ri.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime2(7)");

            builder.Property(ri => ri.UpdatedAt)
                .IsRequired()
                .HasColumnType("datetime2(7)");

            // Relationships
            builder.HasOne(ri => ri.Recipe)
                .WithMany(r => r.RecipeIngredients)
                .HasForeignKey(ri => ri.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ri => ri.Ingredient)
                .WithMany(i => i.RecipeIngredients)
                .HasForeignKey(ri => ri.IngredientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ri => ri.UnitOfMeasurement)
                .WithMany(u => u.RecipeIngredients)
                .HasForeignKey(ri => ri.UnitOfMeasurementId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
