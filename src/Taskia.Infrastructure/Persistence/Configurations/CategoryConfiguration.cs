using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taskia.Domain.Entities;

namespace Taskia.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.Property(c => c.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.ColorHex)
            .HasMaxLength(7)
            .HasDefaultValue("#6366F1");

        builder.Property(c => c.Icon)
            .HasMaxLength(50);

        builder.HasIndex(c => c.UserId);
    }
}
