namespace SystemBase;

public interface IExecutable<TResult> : IDisposable
{
    ValueTask<TResult> Execute(CancellationToken cancellationToken = default);
}

public interface IExecutable : IExecutable<Unit> {}

public readonly struct Unit : IEquatable<Unit>, IComparable<Unit>
{
    static readonly Unit _value = new();

    public static Unit Value => _value;

    public int CompareTo(Unit other) => 0;

    public bool Equals(Unit other) => true;

    public override bool Equals(object? obj) => true;

    public override int GetHashCode() => 0;

    public static bool operator ==(Unit _, Unit __) => true;

    public static bool operator !=(Unit _, Unit __) => false;
}