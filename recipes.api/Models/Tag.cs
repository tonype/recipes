using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Recipes.Api.Models;

public class Tag
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public ICollection<RecipeTag> RecipeTags { get; set; } = new List<RecipeTag>();

    // Entity configuration
    public class Configuration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("Tags");

            // Primary key
            builder.HasKey(t => t.Id);

            // Properties
            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime2(7)");

            builder.Property(t => t.UpdatedAt)
                .IsRequired()
                .HasColumnType("datetime2(7)");
        }
    }
}
