using System;

namespace Taskia.Domain.Common;

public abstract class AuditableEntity<TId> : Entity<TId>
{
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }

    protected AuditableEntity() { }

    protected AuditableEntity(TId id) : base(id) { }
}
