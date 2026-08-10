using System;
using System.Collections.Generic;
using Taskia.Domain.Common;

namespace Taskia.Domain.Entities;

public class User : AggregateRoot<Guid>
{
    public string Email { get; private set; } = string.Empty;
    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public bool IsEmailVerified { get; private set; }
    public string? EmailVerificationToken { get; private set; }
    public string? PasswordResetToken { get; private set; }
    public DateTime? PasswordResetTokenExpiresAtUtc { get; private set; }

    private readonly List<TaskItem> _tasks = new();
    public IReadOnlyCollection<TaskItem> Tasks => _tasks.AsReadOnly();

    private readonly List<Category> _categories = new();
    public IReadOnlyCollection<Category> Categories => _categories.AsReadOnly();

    private readonly List<Tag> _tags = new();
    public IReadOnlyCollection<Tag> Tags => _tags.AsReadOnly();

    private User() { } // EF Core constructor

    public User(Guid id, string email, string username, string passwordHash) : base(id)
    {
        Email = email.ToLowerInvariant();
        Username = username;
        PasswordHash = passwordHash;
        IsEmailVerified = false;
    }

    public void VerifyEmail()
    {
        IsEmailVerified = true;
        EmailVerificationToken = null;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void SetEmailVerificationToken(string token)
    {
        EmailVerificationToken = token;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void SetPasswordResetToken(string token, DateTime expiresAtUtc)
    {
        PasswordResetToken = token;
        PasswordResetTokenExpiresAtUtc = expiresAtUtc;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void UpdatePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        PasswordResetToken = null;
        PasswordResetTokenExpiresAtUtc = null;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
