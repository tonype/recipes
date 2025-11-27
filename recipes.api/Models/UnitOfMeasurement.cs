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
        }
    }
}
