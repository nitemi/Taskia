using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taskia.Domain.Entities;

namespace Taskia.Infrastructure.Persistence.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("tags");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedNever();

        builder.Property(t => t.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.ColorHex)
            .HasMaxLength(7)
            .HasDefaultValue("#8B5CF6");

        builder.HasIndex(t => t.UserId);
    }
}
