using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Taskia.Application.Common.Interfaces;
using Taskia.Domain.Entities;

namespace Taskia.Infrastructure.Persistence;

public class TaskiaDbContext : DbContext, IApplicationDbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<TaskItem> TaskItems => Set<TaskItem>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Reminder> Reminders => Set<Reminder>();
    public DbSet<RecurrenceRule> RecurrenceRules => Set<RecurrenceRule>();

    public TaskiaDbContext(DbContextOptions<TaskiaDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
