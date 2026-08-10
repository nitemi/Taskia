using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taskia.Domain.Entities;

namespace Taskia.Infrastructure.Persistence.Configurations;

public class ReminderConfiguration : IEntityTypeConfiguration<Reminder>
{
    public void Configure(EntityTypeBuilder<Reminder> builder)
    {
        builder.ToTable("reminders");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedNever();

        builder.Property(r => r.ScheduledAtUtc)
            .IsRequired();

        builder.Property(r => r.IsSent)
            .HasDefaultValue(false);

        builder.HasIndex(r => new { r.ScheduledAtUtc, r.IsSent });
        builder.HasIndex(r => r.TaskItemId);
    }
}
