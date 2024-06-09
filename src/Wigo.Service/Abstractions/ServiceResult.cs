namespace Wigo.Service.Abstractions;

public class ServiceResult<T>
{
    public bool Success { get; private set; }
    public bool Failure => !Success;
    public T Data { get; private set; }
    public string ErrorMessage { get; private set; }

    private ServiceResult(bool success, T result, string errorMessage)
    {
        Success = success;
        Data = result;
        ErrorMessage = errorMessage;
    }

    public static ServiceResult<T> SuccessResult(T result)
    {
        return new ServiceResult<T>(true, result, null);
    }

    public static ServiceResult<T> FailureResult(string errorMessage)
    {
        return new ServiceResult<T>(false, default, errorMessage);
    }
}