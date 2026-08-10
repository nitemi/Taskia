using System;
using Taskia.Domain.Common;
using Taskia.Domain.Enums;

namespace Taskia.Domain.Entities;

public class RecurrenceRule : Entity<Guid>
{
    public RecurrenceType RecurrenceType { get; private set; }
    public int Interval { get; private set; } = 1;
    public int? DaysOfWeekMask { get; private set; }
    public DateTime? EndDateUtc { get; private set; }

    private RecurrenceRule() { }

    public RecurrenceRule(Guid id, RecurrenceType recurrenceType, int interval = 1, int? daysOfWeekMask = null, DateTime? endDateUtc = null) : base(id)
    {
        RecurrenceType = recurrenceType;
        Interval = interval > 0 ? interval : 1;
        DaysOfWeekMask = daysOfWeekMask;
        EndDateUtc = endDateUtc;
    }
}
