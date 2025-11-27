using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Recipes.Api.Models;

public class RecipeTag
{
    public Guid RecipeId { get; set; }
    public Guid TagId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public Recipe Recipe { get; set; } = null!;
    public Tag Tag { get; set; } = null!;

    // Entity configuration
    public class Configuration : IEntityTypeConfiguration<RecipeTag>
    {
        public void Configure(EntityTypeBuilder<RecipeTag> builder)
        {
            builder.ToTable("RecipeTags");

            // Composite primary key
            builder.HasKey(rt => new { rt.RecipeId, rt.TagId });

            // Properties
            builder.Property(rt => rt.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime2(7)");

            builder.Property(rt => rt.UpdatedAt)
                .IsRequired()
                .HasColumnType("datetime2(7)");

            // Relationships
            builder.HasOne(rt => rt.Recipe)
                .WithMany(r => r.RecipeTags)
                .HasForeignKey(rt => rt.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rt => rt.Tag)
                .WithMany(t => t.RecipeTags)
                .HasForeignKey(rt => rt.TagId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
