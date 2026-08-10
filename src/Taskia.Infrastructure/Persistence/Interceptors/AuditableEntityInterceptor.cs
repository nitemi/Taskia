using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Taskia.Application.Common.Interfaces;
using Taskia.Domain.Common;

namespace Taskia.Infrastructure.Persistence.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public AuditableEntityInterceptor(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        var utcNow = _dateTimeProvider.UtcNow;

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (IsAuditable(entry.Entity.GetType()))
            {
                var createdAtProp = entry.Property("CreatedAtUtc");
                var updatedAtProp = entry.Property("UpdatedAtUtc");

                if (entry.State == EntityState.Added)
                {
                    if (createdAtProp.CurrentValue == null || (DateTime)createdAtProp.CurrentValue == default)
                    {
                        createdAtProp.CurrentValue = utcNow;
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    updatedAtProp.CurrentValue = utcNow;
                }
            }
        }
    }

    private static bool IsAuditable(Type? type)
    {
        while (type != null && type != typeof(object))
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(AuditableEntity<>))
            {
                return true;
            }
            type = type.BaseType;
        }
        return false;
    }
}
