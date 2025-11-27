using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Recipes.Api.Models;

public class UnitOfMeasurement
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

    // Entity configuration
    public class Configuration : IEntityTypeConfiguration<UnitOfMeasurement>
    {
        public void Configure(EntityTypeBuilder<UnitOfMeasurement> builder)
        {
            builder.ToTable("UnitsOfMeasurement");

            // Primary key
            builder.HasKey(u => u.Id);

            // Properties
            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime2(7)");

            builder.Property(u => u.UpdatedAt)
                .IsRequired()
                .HasColumnType("datetime2(7)");

            // Seed standard units of measurement
            builder.HasData(
                new UnitOfMeasurement { Id = Guid.NewGuid(), Name = "cup", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new UnitOfMeasurement { Id = Guid.NewGuid(), Name = "tablespoon", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new UnitOfMeasurement { Id = Guid.NewGuid(), Name = "teaspoon", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new UnitOfMeasurement { Id = Guid.NewGuid(), Name = "ounce", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new UnitOfMeasurement { Id = Guid.NewGuid(), Name = "pound", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new UnitOfMeasurement { Id = Guid.NewGuid(), Name = "gram", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new UnitOfMeasurement { Id = Guid.NewGuid(), Name = "kilogram", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new UnitOfMeasurement { Id = Guid.NewGuid(), Name = "milliliter", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new UnitOfMeasurement { Id = Guid.NewGuid(), Name = "liter", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new UnitOfMeasurement { Id = Guid.NewGuid(), Name = "pinch", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new UnitOfMeasurement { Id = Guid.NewGuid(), Name = "dash", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new UnitOfMeasurement { Id = Guid.NewGuid(), Name = "whole", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new UnitOfMeasurement { Id = Guid.NewGuid(), Name = "piece", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new UnitOfMeasurement { Id = Guid.NewGuid(), Name = "slice", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new UnitOfMeasurement { Id = Guid.NewGuid(), Name = "clove", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new UnitOfMeasurement { Id = Guid.NewGuid(), Name = "can", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new UnitOfMeasurement { Id = Guid.NewGuid(), Name = "package", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new UnitOfMeasurement { Id = Guid.NewGuid(), Name = "jar", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new UnitOfMeasurement { Id = Guid.NewGuid(), Name = "bottle", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new UnitOfMeasurement { Id = Guid.NewGuid(), Name = "box", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            );
        }
    }
}
