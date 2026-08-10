using System;

namespace Taskia.Application.Common.Interfaces;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    string? UserEmail { get; }
    bool IsAuthenticated { get; }
}
