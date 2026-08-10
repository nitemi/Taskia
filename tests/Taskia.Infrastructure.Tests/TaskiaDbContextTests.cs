using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Taskia.Application.Common.Interfaces;
using Taskia.Domain.Entities;

using Taskia.Domain.Enums;
using Taskia.Infrastructure.Persistence;
using Taskia.Infrastructure.Persistence.Interceptors;
using Xunit;

namespace Taskia.Infrastructure.Tests;

public class TaskiaDbContextTests
{
    [Fact]
    public async Task SaveChangesAsync_ShouldAutoPopulateCreatedAtUtcViaInterceptor()
    {
        // Arrange
        var fixedTime = new DateTime(2026, 8, 1, 12, 0, 0, DateTimeKind.Utc);
        var dateTimeProviderMock = new Mock<IDateTimeProvider>();
        dateTimeProviderMock.Setup(x => x.UtcNow).Returns(fixedTime);

        var interceptor = new AuditableEntityInterceptor(dateTimeProviderMock.Object);

        var options = new DbContextOptionsBuilder<TaskiaDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .AddInterceptors(interceptor)
            .Options;

        using var context = new TaskiaDbContext(options);

        var user = new User(Guid.NewGuid(), "test@taskia.com", "testuser", "hashed_pass");
        var task = new TaskItem(Guid.NewGuid(), user.Id, "Integration Task", "Testing EF Core", TaskPriority.High);

        // Act
        context.Users.Add(user);
        context.TaskItems.Add(task);
        await context.SaveChangesAsync();

        // Assert
        var savedTask = await context.TaskItems.FirstOrDefaultAsync(t => t.Id == task.Id);
        savedTask.Should().NotBeNull();
        savedTask!.Title.Should().Be("Integration Task");
        savedTask.CreatedAtUtc.Should().Be(fixedTime);
    }
}
