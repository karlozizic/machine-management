namespace MachineManagement.API.Result;

public sealed record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static Error NotFound(string description = "Resource not found") => new("NotFound", description);
    public static Error BadRequest(string description = "Bad request") => new("BadRequest", description);
    public static Error Unauthorized(string description = "Unauthorized") => new("Unauthorized", description);
    public static Error Forbidden(string description = "Forbidden") => new("Forbidden", description);
    public static Error Conflict(string description = "Conflict") => new("Conflict", description);
    public static Error UnprocessableEntity(string description = "Unprocessable entity") => new("UnprocessableEntity", description);
    public static Error TooManyRequests(string description = "Too many requests") => new("TooManyRequests", description);
    public static Error InternalServerError(string description = "Internal server error") => new("InternalServerError", description);
    public static Error ValidationError(string description = "Validation error") => new("ValidationError", description);
}