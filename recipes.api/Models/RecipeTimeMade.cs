using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Recipes.Api.Models;

public class RecipeTimeMade
{
    public Guid RecipeId { get; set; }
    public DateTime MadeAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public Recipe Recipe { get; set; } = null!;

    // Entity configuration
    public class Configuration : IEntityTypeConfiguration<RecipeTimeMade>
    {
        public void Configure(EntityTypeBuilder<RecipeTimeMade> builder)
        {
            builder.ToTable("RecipeTimesMade");

            // Composite primary key
            builder.HasKey(rtm => new { rtm.RecipeId, rtm.MadeAt });

            // Properties
            builder.Property(rtm => rtm.MadeAt)
                .IsRequired()
                .HasColumnType("datetime2(7)");

            builder.Property(rtm => rtm.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime2(7)");

            builder.Property(rtm => rtm.UpdatedAt)
                .IsRequired()
                .HasColumnType("datetime2(7)");

            // Relationships
            builder.HasOne(rtm => rtm.Recipe)
                .WithMany(r => r.RecipeTimesMade)
                .HasForeignKey(rtm => rtm.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
