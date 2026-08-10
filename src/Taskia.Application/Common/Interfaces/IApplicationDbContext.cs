using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Taskia.Domain.Entities;

namespace Taskia.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<TaskItem> TaskItems { get; }
    DbSet<Category> Categories { get; }
    DbSet<Tag> Tags { get; }
    DbSet<Reminder> Reminders { get; }
    DbSet<RecurrenceRule> RecurrenceRules { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
