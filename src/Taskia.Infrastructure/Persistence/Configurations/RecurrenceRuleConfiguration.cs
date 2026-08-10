using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taskia.Domain.Entities;

namespace Taskia.Infrastructure.Persistence.Configurations;

public class RecurrenceRuleConfiguration : IEntityTypeConfiguration<RecurrenceRule>
{
    public void Configure(EntityTypeBuilder<RecurrenceRule> builder)
    {
        builder.ToTable("recurrence_rules");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedNever();

        builder.Property(r => r.RecurrenceType)
            .IsRequired();

        builder.Property(r => r.Interval)
            .HasDefaultValue(1);
    }
}
