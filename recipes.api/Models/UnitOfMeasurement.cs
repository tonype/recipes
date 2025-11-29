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

            // Seed standard units of measurement with stable GUIDs and timestamps
            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            builder.HasData(
                new UnitOfMeasurement { Id = new Guid("cec58f72-bbb8-473e-98ac-a8b153c66f77"), Name = "cup", CreatedAt = seedDate, UpdatedAt = seedDate },
                new UnitOfMeasurement { Id = new Guid("e507aecc-50a0-45d7-b43a-77bb9b3b701c"), Name = "tablespoon", CreatedAt = seedDate, UpdatedAt = seedDate },
                new UnitOfMeasurement { Id = new Guid("fecd3df0-1165-4c99-9ea3-589867ae8e19"), Name = "teaspoon", CreatedAt = seedDate, UpdatedAt = seedDate },
                new UnitOfMeasurement { Id = new Guid("537debe5-cdfc-482e-8fb2-6e0956524976"), Name = "ounce", CreatedAt = seedDate, UpdatedAt = seedDate },
                new UnitOfMeasurement { Id = new Guid("d38a828c-79d9-44f7-a912-d912b97cd1ec"), Name = "pound", CreatedAt = seedDate, UpdatedAt = seedDate },
                new UnitOfMeasurement { Id = new Guid("00a0409c-8239-4118-9938-8feb598cfe31"), Name = "gram", CreatedAt = seedDate, UpdatedAt = seedDate },
                new UnitOfMeasurement { Id = new Guid("eeed0d4f-c3df-4665-8712-ba392490dce6"), Name = "kilogram", CreatedAt = seedDate, UpdatedAt = seedDate },
                new UnitOfMeasurement { Id = new Guid("9a0aa1c9-658b-4769-abd2-7280f6cb9e38"), Name = "milliliter", CreatedAt = seedDate, UpdatedAt = seedDate },
                new UnitOfMeasurement { Id = new Guid("169ac0d3-a029-471b-a285-87f04f6c80ca"), Name = "liter", CreatedAt = seedDate, UpdatedAt = seedDate },
                new UnitOfMeasurement { Id = new Guid("4aed97c5-f741-4a2e-bc8c-bccb6e1d5998"), Name = "pinch", CreatedAt = seedDate, UpdatedAt = seedDate },
                new UnitOfMeasurement { Id = new Guid("efc6654e-9a41-405b-8e2a-14f8e543c3c3"), Name = "dash", CreatedAt = seedDate, UpdatedAt = seedDate },
                new UnitOfMeasurement { Id = new Guid("e93ac73d-dc29-43ba-b838-2bc23f521e3b"), Name = "whole", CreatedAt = seedDate, UpdatedAt = seedDate },
                new UnitOfMeasurement { Id = new Guid("eabd7030-1f6d-493f-92b8-1e0b4a25b67f"), Name = "piece", CreatedAt = seedDate, UpdatedAt = seedDate },
                new UnitOfMeasurement { Id = new Guid("33240855-a90f-4eae-aed9-e5505a760ec7"), Name = "slice", CreatedAt = seedDate, UpdatedAt = seedDate },
                new UnitOfMeasurement { Id = new Guid("b000f375-1dae-4200-a0bf-d48614dc7b0d"), Name = "clove", CreatedAt = seedDate, UpdatedAt = seedDate },
                new UnitOfMeasurement { Id = new Guid("dc21eba3-b1f6-4575-8050-739290e6cb4e"), Name = "can", CreatedAt = seedDate, UpdatedAt = seedDate },
                new UnitOfMeasurement { Id = new Guid("b5bdab3f-077d-442b-9b4e-e4aa0761e7d4"), Name = "package", CreatedAt = seedDate, UpdatedAt = seedDate },
                new UnitOfMeasurement { Id = new Guid("4fea1e82-8f1d-4b03-95f1-dd167c18c41c"), Name = "jar", CreatedAt = seedDate, UpdatedAt = seedDate },
                new UnitOfMeasurement { Id = new Guid("1289a211-b2a9-4d05-89f6-b30979271b16"), Name = "bottle", CreatedAt = seedDate, UpdatedAt = seedDate },
                new UnitOfMeasurement { Id = new Guid("ece62990-f684-4ab9-843f-4d2d076fedf5"), Name = "box", CreatedAt = seedDate, UpdatedAt = seedDate }
            );
        }
    }
}
