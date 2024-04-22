namespace SystemBase;

public interface IExecutable<TResult> : IDisposable
{
    ValueTask<TResult> Execute(CancellationToken cancellationToken = default);
}

public interface IExecutable : IExecutable<Unit> {}

public readonly struct Unit
{
    static readonly Unit _value = new();

    public static Unit Value => _value;
}