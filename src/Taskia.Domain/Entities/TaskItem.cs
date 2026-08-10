using System;
using System.Collections.Generic;
using Taskia.Domain.Common;
using Taskia.Domain.Enums;
using Taskia.Domain.Exceptions;
using TaskStatus = Taskia.Domain.Enums.TaskStatus;

namespace Taskia.Domain.Entities;

public class TaskItem : AggregateRoot<Guid>
{
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public TaskStatus Status { get; private set; } = TaskStatus.Pending;
    public TaskPriority Priority { get; private set; } = TaskPriority.Medium;
    public DateTime? DueDateUtc { get; private set; }

    public Guid? CategoryId { get; private set; }
    public Category? Category { get; private set; }

    public Guid? RecurrenceRuleId { get; private set; }
    public RecurrenceRule? RecurrenceRule { get; private set; }

    public bool IsArchived { get; private set; }

    private readonly List<Tag> _tags = new();
    public IReadOnlyCollection<Tag> Tags => _tags.AsReadOnly();

    private readonly List<Reminder> _reminders = new();
    public IReadOnlyCollection<Reminder> Reminders => _reminders.AsReadOnly();

    private TaskItem() { } // EF Core

    public TaskItem(
        Guid id,
        Guid userId,
        string title,
        string? description = null,
        TaskPriority priority = TaskPriority.Medium,
        DateTime? dueDateUtc = null,
        Guid? categoryId = null,
        Guid? recurrenceRuleId = null) : base(id)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Task title cannot be empty.", "invalid_title");

        UserId = userId;
        Title = title;
        Description = description;
        Priority = priority;
        DueDateUtc = dueDateUtc;
        CategoryId = categoryId;
        RecurrenceRuleId = recurrenceRuleId;
        Status = TaskStatus.Pending;
        IsArchived = false;
    }

    public void Update(
        string title,
        string? description,
        TaskPriority priority,
        DateTime? dueDateUtc,
        Guid? categoryId)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Task title cannot be empty.", "invalid_title");

        Title = title;
        Description = description;
        Priority = priority;
        DueDateUtc = dueDateUtc;
        CategoryId = categoryId;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Complete()
    {
        Status = TaskStatus.Completed;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void MarkPending()
    {
        Status = TaskStatus.Pending;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void ToggleArchive()
    {
        IsArchived = !IsArchived;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void AddTag(Tag tag)
    {
        if (!_tags.Contains(tag))
        {
            _tags.Add(tag);
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }

    public void RemoveTag(Tag tag)
    {
        if (_tags.Remove(tag))
        {
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }

    public void AddReminder(Reminder reminder)
    {
        _reminders.Add(reminder);
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
