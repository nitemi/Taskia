using System;
using Taskia.Domain.Common;
using Taskia.Domain.Enums;

namespace Taskia.Domain.Entities;

public class Reminder : AuditableEntity<Guid>
{
    public Guid TaskItemId { get; private set; }
    public TaskItem TaskItem { get; private set; } = null!;
    public DateTime ScheduledAtUtc { get; private set; }
    public ReminderType ReminderType { get; private set; }
    public bool IsSent { get; private set; }
    public DateTime? SnoozedUntilUtc { get; private set; }

    private Reminder() { }

    public Reminder(Guid id, Guid taskItemId, DateTime scheduledAtUtc, ReminderType reminderType = ReminderType.LocalNotification) : base(id)
    {
        TaskItemId = taskItemId;
        ScheduledAtUtc = scheduledAtUtc;
        ReminderType = reminderType;
        IsSent = false;
    }

    public void MarkAsSent()
    {
        IsSent = true;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Snooze(TimeSpan duration)
    {
        SnoozedUntilUtc = DateTime.UtcNow.Add(duration);
        ScheduledAtUtc = SnoozedUntilUtc.Value;
        IsSent = false;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
