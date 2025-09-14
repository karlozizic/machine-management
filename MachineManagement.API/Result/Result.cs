namespace MachineManagement.API.Result;

public class Result
{
    public bool IsSuccess { get; }
    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);

    public static implicit operator bool(Result result) => result.IsSuccess;
}

public class Result<T> : Result
{
    public T Value { get; }

    private Result(bool isSuccess, T value, Error error) : base(isSuccess, error)
    {
        Value = value;
    }

    public static Result<T> Success(T value) => new(true, value, Error.None);
    public new static Result<T> Failure(Error error) => new(false, default, error);

    public static implicit operator T(Result<T> result) => result.Value;
    public static implicit operator Error(Result<T> result) => result.Error;
    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(Error error) => Failure(error);
}