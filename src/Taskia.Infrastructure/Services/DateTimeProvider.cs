using System;
using Taskia.Application.Common.Interfaces;

namespace Taskia.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
