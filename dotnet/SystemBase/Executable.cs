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

public readonly record struct ExecutionResultProfile
{
    public string Key { get; init; }
    public bool IsEmpty { get; init;  }
    public Type Type { get; init; }
}

public interface IExecutionResultRegistry
{
    IExecutionResultRegistry Register<T>(string key, bool isEmpty = false) where T : IExecutionResult;
    bool TryGetProfile<T>(out ExecutionResultProfile profile);
}

sealed class ExecutionResultRegistry : IExecutionResultRegistry
{
    readonly Dictionary<Type, ExecutionResultProfile> _entries = [];

    public bool TryGetProfile<T>(out ExecutionResultProfile profile)
    {
        return _entries.TryGetValue(typeof(T), out profile);
    }

    public IExecutionResultRegistry Register<T>(string key, bool isEmpty = false) where T : IExecutionResult
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return this;
        }

        var type = typeof(T);
        _entries.TryAdd(type, new ExecutionResultProfile { 
            Key = key, 
            IsEmpty = isEmpty, 
            Type = type
        });

        return this;
    }
}
