using System;
using System.Collections.Generic;
using Taskia.Domain.Common;

namespace Taskia.Domain.Entities;

public class Category : AuditableEntity<Guid>
{
    public string Name { get; private set; } = string.Empty;
    public string ColorHex { get; private set; } = "#6366F1";
    public string? Icon { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    private readonly List<TaskItem> _tasks = new();
    public IReadOnlyCollection<TaskItem> Tasks => _tasks.AsReadOnly();

    private Category() { }

    public Category(Guid id, Guid userId, string name, string colorHex = "#6366F1", string? icon = null) : base(id)
    {
        UserId = userId;
        Name = name;
        ColorHex = colorHex;
        Icon = icon;
    }

    public void Update(string name, string colorHex, string? icon)
    {
        Name = name;
        ColorHex = colorHex;
        Icon = icon;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
