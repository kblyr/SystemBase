namespace SystemBase;

public class UninitializedPropertyException : Exception
{
    public string PropertyName { get; }

    public UninitializedPropertyException(string propertyName)
    {
        PropertyName = propertyName;
    }

    public UninitializedPropertyException(string propertyName, string? message) : base(message)
    {
        PropertyName = propertyName;
    }

    public UninitializedPropertyException(string propertyName, string? message, Exception? innerException) : base(message, innerException)
    {
        PropertyName = propertyName;
    }
}