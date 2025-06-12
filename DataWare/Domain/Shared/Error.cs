namespace Domain.Shared;

public class Error : IEquatable<Error>
{
    public static readonly Error None = Error.Failure(string.Empty, string.Empty);
    public static readonly Error NullValue = Error.Failure("Error.NullValue", "The specified result value is null.");

    private Error(string code, string message, ErrorType type)
    {
        Code = code;
        Message = message;
        Type = type;
    }

    public string Code { get; }

    public string Message { get; }

    public ErrorType Type { get; }

    public static Error Failure(string code, string message) => new Error(code, message, ErrorType.Failure);
    public static Error NotFound(string code, string message) => new Error(code, message, ErrorType.NotFound);
    public static Error Conflict(string code, string message) => new Error(code, message, ErrorType.Conflict);
    public static Error Validation(string code, string message) => new Error(code, message, ErrorType.Validation);
    public static Error Unauthorized(string code, string message) => new Error(code, message, ErrorType.Unauthorized);
    public static Error Forbidden(string code, string message) => new Error(code, message, ErrorType.Forbidden);

    public static bool operator ==(Error? a, Error? b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }
    public static bool operator !=(Error? a, Error? b) => !(a == b);

    public virtual bool Equals(Error? other)
    {
        if (other is null)
        {
            return false;
        }

        return Code == other.Code && Message == other.Message;
    }
    public override bool Equals(object? obj) => obj is Error error && Equals(error);

    public static implicit operator string(Error error) => error.Code;
    public override int GetHashCode() => HashCode.Combine(Code, Message);
    public override string ToString() => Code;
}