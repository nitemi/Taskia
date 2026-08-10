namespace Taskia.Application.Common.Models;

public record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("error.null", "A null value was provided.");
    public static readonly Error NotFound = new("error.not_found", "The requested resource was not found.");
}
