namespace GameOfLife.Application.Results;

public class Error
{
    public string Code { get; }
    public string Message { get; }

    private Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public static Error None() => new("None", string.Empty);

    public static Error Null(string message) =>
        new("NullError", message);

    public static Error Validation(string message) =>
        new("ValidationError", message);

    public static Error Unexpected(string message) =>
        new("UnexpectedError", message);

    public override string ToString() => $"{Code}: {Message}";
}
