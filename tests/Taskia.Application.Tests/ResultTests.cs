using System;
using FluentAssertions;
using Taskia.Application.Common.Models;
using Xunit;

namespace Taskia.Application.Tests;

public class ResultTests
{
    [Fact]
    public void Success_ShouldCreateSuccessfulResult()
    {
        // Act
        var result = Result.Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Error.Should().Be(Error.None);
    }

    [Fact]
    public void Failure_ShouldCreateFailedResultWithError()
    {
        // Arrange
        var error = new Error("test.code", "Test error description");

        // Act
        var result = Result.Failure(error);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void SuccessGeneric_WithValue_ShouldReturnWithValue()
    {
        // Act
        var result = Result.Success(42);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public void FailureGeneric_AccessingValue_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var result = Result.Failure<string>(Error.NotFound);

        // Act
        Action act = () => _ = result.Value;

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
}
