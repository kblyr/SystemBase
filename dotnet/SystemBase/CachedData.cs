namespace SystemBase;

public sealed class CachedData<T>
{
    T? _value;

    public CachedData() {}

    public CachedData(T defaultValue) => _value = defaultValue;

    public bool TryGet(out T value)
    {
        value = _value ?? default!;
        return _value is not null;
    }

    public T Set(T value)
    {
        _value = value;
        return value;
    }
}

public sealed class KeyedCachedData<TKey, TValue> where TKey : notnull
{
    readonly System.Collections.Concurrent.ConcurrentDictionary<TKey, TValue> _source = new();
    public int Count => _source.Count;

    public bool TryGet(TKey key, out TValue value)
    {
        return _source.TryGetValue(key, out value!);
    }

    public TValue Set(TKey key, TValue value)
    {
        _source.AddOrUpdate(key, value, (existingKey, existingValue) => value);
        return value;
    }
}