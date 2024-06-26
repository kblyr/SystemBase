namespace SystemBase;

public class FailedToGetAPIResponseTypeKeyException : Exception
{
    public Type ResponseType { get; }

    public FailedToGetAPIResponseTypeKeyException(Type responseType)
    {
        ResponseType = responseType;
    }

    public FailedToGetAPIResponseTypeKeyException(Type responseType, string? message) : base(message)
    {
        ResponseType = responseType;
    }

    public FailedToGetAPIResponseTypeKeyException(Type responseType, string? message, Exception? innerException) : base(message, innerException)
    {
        ResponseType = responseType;
    }
}
