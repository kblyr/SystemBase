namespace SystemBase;

public interface IExecutable : IDisposable
{
    ValueTask<IExecutionResult> Execute(CancellationToken cancellationToken = default);
}

public interface IExecutionResult {}

public record ExecutionSuccess : IExecutionResult
{
    static readonly ExecutionSuccess _instance = new();

    public static ExecutionSuccess Instance => _instance;

    protected ExecutionSuccess() {}
}

public record ExecutionError : IExecutionResult
{
    static readonly ExecutionError _instance = new();

    public static ExecutionError Instance => _instance;

    protected ExecutionError() {}
}

[AttributeUsage(AttributeTargets.Class)]
public class ExecutionResultAttribute(string key, bool isEmpty = false) : Attribute
{
    readonly string _key = key;
    readonly bool _isEmpty = isEmpty;

    public string Key => _key;

    public bool IsEmpty => _isEmpty;
}