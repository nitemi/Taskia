using System;
using System.Collections.Generic;
using Taskia.Domain.Common;

namespace Taskia.Domain.Entities;

public class Tag : Entity<Guid>
{
    public string Name { get; private set; } = string.Empty;
    public string ColorHex { get; private set; } = "#8B5CF6";
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    private readonly List<TaskItem> _tasks = new();
    public IReadOnlyCollection<TaskItem> Tasks => _tasks.AsReadOnly();

    private Tag() { }

    public Tag(Guid id, Guid userId, string name, string colorHex = "#8B5CF6") : base(id)
    {
        UserId = userId;
        Name = name;
        ColorHex = colorHex;
    }

    public void Update(string name, string colorHex)
    {
        Name = name;
        ColorHex = colorHex;
    }
}
