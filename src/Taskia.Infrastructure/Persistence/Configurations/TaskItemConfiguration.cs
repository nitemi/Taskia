using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taskia.Domain.Entities;

namespace Taskia.Infrastructure.Persistence.Configurations;

public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.ToTable("task_items");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedNever();

        builder.Property(t => t.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(t => t.Description)
            .HasColumnType("text");

        builder.Property(t => t.Status)
            .IsRequired();

        builder.Property(t => t.Priority)
            .IsRequired();

        builder.Property(t => t.IsArchived)
            .HasDefaultValue(false);

        builder.HasIndex(t => new { t.UserId, t.Status, t.DueDateUtc });
        builder.HasIndex(t => t.IsArchived);

        builder.HasOne(t => t.Category)
            .WithMany(c => c.Tasks)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(t => t.RecurrenceRule)
            .WithMany()
            .HasForeignKey(t => t.RecurrenceRuleId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(t => t.Tags)
            .WithMany(tag => tag.Tasks)
            .UsingEntity(j => j.ToTable("task_item_tags"));

        builder.HasMany(t => t.Reminders)
            .WithOne(r => r.TaskItem)
            .HasForeignKey(r => r.TaskItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
