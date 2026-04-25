namespace GameOfLife.Application.Results;

public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    private readonly T? _value;
    public T Value =>
        IsSuccess
            ? _value!
            : throw new InvalidOperationException("Cannot access Value when result is failure");

    public Error Error { get; }

    private Result(T value)
    {
        IsSuccess = true;
        _value = value;
        Error = Error.None();
    }

    private Result(Error error)
    {
        IsSuccess = false;
        _value = default;
        Error = error;
    }

    public static Result<T> Success(T value) => new(value);

    public static Result<T> Fail(Error error) => new(error);
}