namespace FileUploaderPartA.Core.Shared;

public class Result<T>
{
    private Result(T? data, bool isSuccess, string message)
    {
        Data = data;
        IsSuccess = isSuccess;
        Message = message;
    }

    public T? Data { get; }
    public bool IsSuccess { get; }
    public string Message { get; }

    public static Result<T> Success(T data, string message = "Success")
    {
        return new Result<T>(data, true, message);
    }

    public static Result<T> Failure(string message)
    {
        return new Result<T>(default, false, message);
    }
}