using System;
using FluentAssertions;
using Taskia.Domain.Entities;
using Taskia.Domain.Enums;
using Taskia.Domain.Exceptions;
using Xunit;
using TaskStatus = Taskia.Domain.Enums.TaskStatus;

namespace Taskia.Domain.Tests;

public class TaskItemTests
{
    [Fact]
    public void Constructor_WithValidArguments_ShouldInitializeTaskItem()
    {
        // Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var title = "Design Clean Architecture Solution";
        var description = "Scaffold layers and entities";

        // Act
        var task = new TaskItem(id, userId, title, description, TaskPriority.High);

        // Assert
        task.Id.Should().Be(id);
        task.UserId.Should().Be(userId);
        task.Title.Should().Be(title);
        task.Description.Should().Be(description);
        task.Priority.Should().Be(TaskPriority.High);
        task.Status.Should().Be(TaskStatus.Pending);
        task.IsArchived.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_WithEmptyOrNullTitle_ShouldThrowDomainException(string? invalidTitle)
    {
        // Arrange & Act
        Action act = () => new TaskItem(Guid.NewGuid(), Guid.NewGuid(), invalidTitle!);

        // Assert
        act.Should().Throw<DomainException>()
           .WithMessage("Task title cannot be empty.");
    }

    [Fact]
    public void Complete_ShouldUpdateStatusToCompleted()
    {
        // Arrange
        var task = new TaskItem(Guid.NewGuid(), Guid.NewGuid(), "Test Task");

        // Act
        task.Complete();

        // Assert
        task.Status.Should().Be(TaskStatus.Completed);
        task.UpdatedAtUtc.Should().NotBeNull();
    }

    [Fact]
    public void ToggleArchive_ShouldInvertIsArchivedFlag()
    {
        // Arrange
        var task = new TaskItem(Guid.NewGuid(), Guid.NewGuid(), "Test Task");

        // Act & Assert 1
        task.ToggleArchive();
        task.IsArchived.Should().BeTrue();

        // Act & Assert 2
        task.ToggleArchive();
        task.IsArchived.Should().BeFalse();
    }
}
